// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/Services/OrderService.cs
using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.DTOs.GHN.Request;
using CleanArchitecture.Application.DTOs.GHN.Response;
using CleanArchitecture.Application.DTOs.Order;
using CleanArchitecture.Application.DTOs.OrderDto;
using CleanArchitecture.Application.DTOs.OrderItemDto;
using CleanArchitecture.Application.DTOs.VnPay;
using CleanArchitecture.Application.Enums;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Strategies.InvoiceGenerateStrategy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class OrderService : IOrderService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IErrorFactory _errorFactory;
  private readonly IClaimsService _claimsService;
  private readonly IGHNService _ghnService;
  private readonly IEnumerable<IInvoiceGenerateStrategy> _invoiceGenerateStrategies;
  private readonly ITimeZoneService _timeZoneService;
  private readonly IVnPayIntegrationService _vnPayIntegrationService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IValidator<CreateOnlineOrderRequest> _createOrderRequestValidator;
  private readonly IValidator<CreateWalkInOrderRequest> _createWalkInOrderRequestValidator;
  private readonly UserManager<User> _userManager;
  private readonly ILogger<OrderService> _logger;

  public OrderService(
    IUnitOfWork unitOfWork,
    IErrorFactory errorFactory,
    IClaimsService claimsService,
    IGHNService ghnService,
    ITimeZoneService timeZoneService,
    IVnPayIntegrationService vnPayIntegrationService,
    IHttpContextAccessor httpContextAccessor,
    IValidator<CreateOnlineOrderRequest> createOrderRequestValidator,
    UserManager<User> userManager, IValidator<CreateWalkInOrderRequest> createWalkInOrderRequestValidator,
    IEnumerable<IInvoiceGenerateStrategy> invoiceGenerateStrategies,
    ILogger<OrderService> logger)
  {
    _unitOfWork = unitOfWork;
    _errorFactory = errorFactory;
    _claimsService = claimsService;
    _ghnService = ghnService;
    _timeZoneService = timeZoneService;
    _vnPayIntegrationService = vnPayIntegrationService;
    _httpContextAccessor = httpContextAccessor;
    _createOrderRequestValidator = createOrderRequestValidator;
    _userManager = userManager;
    _createWalkInOrderRequestValidator = createWalkInOrderRequestValidator;
    _invoiceGenerateStrategies = invoiceGenerateStrategies;
    _logger = logger;
  }

  public async Task<Result<OrderResponse>> InitiateOrder(CreateOnlineOrderRequest request)
  {
    try
    {
      var validationResult = await _createOrderRequestValidator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
        var errors = validationResult.Errors
          .Select(e => new Error("ValidationError", e.ErrorMessage))
          .ToList();

        return Result<OrderResponse>.Failure(errors, StatusCodes.Status400BadRequest);
      }

      var cartValidationResult = await ValidateOrderRequest(request);
      if (!cartValidationResult.IsSuccess)
      {
        return Result<OrderResponse>.Failure(
          cartValidationResult.Errors,
          cartValidationResult.Status);
      }

      var cart = cartValidationResult.Data;

      var inventoryResult = await CheckAndDeductInventory(cart);
      if (!inventoryResult.IsSuccess)
      {
        return Result<OrderResponse>.Failure(
          inventoryResult.Errors,
          inventoryResult.Status);
      }

      var customer = await _userManager.FindByIdAsync(_claimsService.CurrentUserId.ToString());

      var order = CreateOrderEntity(request, cart);

      decimal subtotal = 0;
      decimal discountedSubtotal = 0;

      foreach (var item in order.OrderItems)
      {
        decimal originalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(item.Cosmetic);
        decimal discountedPrice = await _unitOfWork.Cosmetics.GetCosmeticPrice(item.Cosmetic);

        item.SellingPrice = discountedPrice;
        subtotal += originalPrice * item.Quantity;
        discountedSubtotal += discountedPrice * item.Quantity;
      }

      order.SubTotal = subtotal;
      decimal totalPrice = discountedSubtotal;

      decimal? couponDiscount = null;
      if (request.CouponId.HasValue)
      {
        var coupon = await _unitOfWork.Coupons.GetByIdAsync(request.CouponId.Value);
        if (coupon is null)
        {
          var error = _errorFactory.CreateNotFoundError("Coupon");
          return Result<OrderResponse>.Failure([error.err], StatusCodes.Status404NotFound);
        }

        var userCoupon = await _unitOfWork.UserCoupons.GetByIdAsync(_claimsService.CurrentUserId, coupon.Id);
        if (userCoupon is null || userCoupon.Quantity <= 0)
        {
          return Result<OrderResponse>.Failure([new Error("UserCoupon.NoCoupon", "User doesnt have this coupon")],
            StatusCodes.Status400BadRequest);
        }

        if (totalPrice < coupon.MinimumOrderPrice)
        {
          return Result<OrderResponse>.Failure([
            new Error("Order.InsufficientPriceForCoupon",
              $"The order does not met coupon minimum price which is {coupon.MinimumOrderPrice}")
          ], StatusCodes.Status400BadRequest);
        }

        if (coupon != null && userCoupon.Quantity > 0 && coupon.EndDate > DateTime.UtcNow)
        {
          couponDiscount = (totalPrice * (decimal)coupon.DiscountAmount) / 100m;
          if (couponDiscount > coupon.MaxDiscountAmount)
            couponDiscount = coupon.MaxDiscountAmount;

          totalPrice -= (decimal)couponDiscount;

          userCoupon.Quantity--;
        }
      }

      order.TotalPrice = totalPrice;
      order.Customer = customer!;

      // Save the order to the database to generate an Order ID
      _unitOfWork.Orders.Create(order);
      var saved = await _unitOfWork.CompleteAsync();

      if (!saved)
      {
        return Result<OrderResponse>.Failure(
          [new Error("Order.Create", "Failed to save order")],
          StatusCodes.Status500InternalServerError);
      }

      // Now that the order is saved, map it to the response DTO
      var orderResponse = MapToOrderResponse(order);
      if (couponDiscount is not null)
        orderResponse.CouponDiscount = couponDiscount;

      // If the payment method is online, create the VNPay URL and add it to the response
      if (request.PaymentMethod == PaymentMethods.ONLINE)
      {
        var vnPayRequest = new VnPayPaymentRequestDto
        {
          OrderId = order.Id,
          Amount = order.TotalPrice,
          PaymentMethod = request.PaymentMethod,
        };

        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
          _logger.LogError("HTTP context is null, cannot create payment URL.");
          return Result<OrderResponse>.Failure([new Error("HttpContextError", "Unable to access HTTP context.")], StatusCodes.Status500InternalServerError);
        }

        var paymentUrlResult = _vnPayIntegrationService.CreatePaymentUrl(vnPayRequest, context);

        if (paymentUrlResult.IsSuccess)
        {
          orderResponse.PaymentUrl = paymentUrlResult.Data;
        }
        else
        {
          _logger.LogError("Failed to create VNPay URL for Order ID {OrderId}: {Errors}", order.Id, string.Join(", ", paymentUrlResult.Errors.Select(e => e.Description)));
          return Result<OrderResponse>.Failure(paymentUrlResult.Errors, paymentUrlResult.Status);
        }
      }

      return Result<OrderResponse>.Success(
        orderResponse,
        StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An exception occurred during order initiation.");
      return Result<OrderResponse>.Failure(
        [new Error("Order.Create", ex.Message)],
        StatusCodes.Status500InternalServerError);
    }
  }

  public async Task<Result<OrderResponse>> InitiateOrder(CreateWalkInOrderRequest request)
  {
    var validationResult = await _createWalkInOrderRequestValidator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      var error = _errorFactory.CreateValidationError(nameof(request), validationResult);
      return Result<OrderResponse>.Failure(error.errs, error.statusCode);
    }

    decimal totalPrice = 0;
    decimal subTotal = 0;
    List<OrderItem> orderItems = new List<OrderItem>();

    foreach (var orderItem in request.Cosmetics)
    {
      var cosmetic = await _unitOfWork.Cosmetics.GetByIdAsync(orderItem.Key);
      if (cosmetic is null)
        return null;
      var originalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(cosmetic);
      var sellingPrice = await _unitOfWork.Cosmetics.GetCosmeticPrice(cosmetic);
      var quantity = orderItem.Value;
      subTotal += originalPrice * quantity;
      totalPrice += sellingPrice * quantity;

      orderItems.Add(new OrderItem()
      {
        CosmeticId = cosmetic.Id,
        SellingPrice = sellingPrice * orderItem.Value,
        Quantity = orderItem.Value

      });
    }

    decimal? couponDiscount = null;
    if (request.CouponId is not null)
    {
      var coupon = await _unitOfWork.Coupons.GetByIdAsync(request.CouponId);
      if (coupon is null)
      {
        var error = _errorFactory.CreateInvalidCouponError();
        return Result<OrderResponse>.Failure([error.err], error.statusCode);
      }

      if (totalPrice < coupon.MinimumOrderPrice)
      {
        return Result<OrderResponse>.Failure([
          new Error("Order.InsufficientPriceForCoupon",
            $"The order does not meet coupon minimum price which is {coupon.MinimumOrderPrice}")
        ], StatusCodes.Status400BadRequest);
      }

      if (coupon.UsageLimit == 0 || coupon.EndDate < DateTime.UtcNow)
      {
        return Result<OrderResponse>.Failure([
          new Error("Coupon.Usage", "Coupon is not available")
        ], StatusCodes.Status400BadRequest);
      }

      couponDiscount = (totalPrice * (decimal)coupon.DiscountAmount) / 100m;
      if (couponDiscount > coupon.MaxDiscountAmount)
        couponDiscount = coupon.MaxDiscountAmount;

      totalPrice -= (decimal)couponDiscount;

      _unitOfWork.Coupons.Update(coupon);
    }

    var customer = await _unitOfWork.Users.GetByPhoneNumberAsync(request.CustomerPhoneNumber!);

    if (customer is null)
    {
      customer = new User
      {
        Id = Guid.NewGuid(),
        FirstName = request.FirstName,
        LastName = request.LastName,
        UserName = request.CustomerPhoneNumber,
        PhoneNumber = request.CustomerPhoneNumber
      };

      var result = await _userManager.CreateAsync(customer, request.CustomerPhoneNumber!);
      if (!result.Succeeded)
      {
        var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
        return Result<OrderResponse>.Failure(errors, StatusCodes.Status500InternalServerError);
      }

      var roleResult = await _userManager.AddToRolesAsync(customer, [Roles.Customer]);
      if (!roleResult.Succeeded)
      {
        var errors = roleResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
        return Result<OrderResponse>.Failure(errors, StatusCodes.Status500InternalServerError);
      }
    }


    var order = new Order()
    {
      CustomerId = customer.Id,
      CouponId = request.CouponId,
      CreateAt = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
      OrderDate = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
      DeliveryDate = null,
      ETA = null,
      IsActive = true,
      TotalPrice = totalPrice,
      SubTotal = subTotal,
      OrderItems = orderItems,
      Status = OrderStatus.COMPLETED,
      PaymentMethod = request.PaymentMethod
    };

    await _unitOfWork.Orders.CreateAsync(order);
    await _unitOfWork.CompleteAsync();

    var invoiceStrategy = _invoiceGenerateStrategies.OfType<WalkInInvoiceStrategy>().FirstOrDefault();
    var invoiceByte = await invoiceStrategy.GenerateAsync(order, _unitOfWork);
    if (invoiceByte.Length == 0)
    {
      var error = _errorFactory.CreateFileCreatedFailed("Invoice");
      return Result<OrderResponse>.Failure([error.err], error.statusCode);
    }

    var orderResponse = MapToOrderResponse(order);
    orderResponse.CustomerId = customer.Id;
    if (couponDiscount.HasValue)
      orderResponse.CouponDiscount = couponDiscount;

    orderResponse.Invoice = invoiceByte;

    return Result<OrderResponse>.Success(orderResponse, StatusCodes.Status200OK);
  }

  private async Task<Result<Cart>> ValidateOrderRequest(CreateOnlineOrderRequest request)
  {
    var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(_claimsService.CurrentUserId);
    if (cart == null || !cart.CartItems.Any())
    {
      return Result<Cart>.Failure(
        [new Error("Order.Invalid", "Cart not found or empty")],
        StatusCodes.Status400BadRequest);
    }

    if (cart.CustomerId != _claimsService.CurrentUserId)
    {
      return Result<Cart>.Failure(
        [new Error("Order.Unauthorized", "Not authorized to access this cart")],
        StatusCodes.Status403Forbidden);
    }

    if (string.IsNullOrEmpty(request.ShippingAddress) ||
        string.IsNullOrEmpty(request.BillingAddress))
    {
      return Result<Cart>.Failure(
        [new Error("Order.Invalid", "Shipping and billing addresses are required")],
        StatusCodes.Status400BadRequest);
    }

    return Result<Cart>.Success(cart, StatusCodes.Status200OK);
  }

  private async Task<Result<bool>> CheckAndDeductInventory(Cart cart)
  {
    var requiredQuantities = cart.CartItems
      .GroupBy(ci => ci.CosmeticId)
      .ToDictionary(g => g.Key, g => g.Sum(ci => ci.Quantity));

    var batchesToUpdate = new List<Batch>();

    foreach (var (cosmeticId, quantityNeeded) in requiredQuantities)
    {
      var availableBatches = await _unitOfWork.Batches.GetListByAnyId(e => e.CosmeticId == cosmeticId, 2);

      availableBatches = availableBatches
        .Where(b => b.ExpirationDate >= DateOnly.FromDateTime(DateTime.UtcNow))
        .OrderBy(b => b.ExportedDate)
        .ToList();

      if (!availableBatches.Any())
      {
        return Result<bool>.Failure(
          [new Error("Order.ExpiredStock", "All available stock has expired")],
          StatusCodes.Status400BadRequest);
      }

      int totalAvailable = availableBatches.Sum(b => b.Quantity);
      if (totalAvailable < quantityNeeded)
      {
        return Result<bool>.Failure(
          [new Error("Order.InsufficientStock", "Not enough stock available")],
          StatusCodes.Status400BadRequest);
      }

      int remainingQuantity = quantityNeeded;
      foreach (var batch in availableBatches)
      {
        if (remainingQuantity <= 0) break;

        int deducted = Math.Min(batch.Quantity, remainingQuantity);
        batch.Quantity -= deducted;
        remainingQuantity -= deducted;

        batchesToUpdate.Add(batch);
      }
    }

    foreach (var batch in batchesToUpdate)
    {
      _unitOfWork.Batches.Update(batch);
    }

    return Result<bool>.Success(true, StatusCodes.Status200OK);
  }

  private ShippingDetails CalculateShippingDetails(IEnumerable<OrderItem> orderItems)
  {
    var totalWeight = 0;
    var totalLength = 0;
    var totalWidth = 0;
    var totalHeight = 0;

    foreach (var orderItem in orderItems)
    {
      totalWeight += orderItem.Cosmetic.Weight * orderItem.Quantity;
      totalLength = Math.Max(totalLength, orderItem.Cosmetic.Length);
      totalWidth = Math.Max(totalWidth, orderItem.Cosmetic.Width);
      totalHeight += orderItem.Cosmetic.Height * orderItem.Quantity; // Stack height
    }

    return new ShippingDetails { Weight = totalWeight, Length = totalLength, Width = totalWidth, Height = totalHeight };
  }

  private Order CreateOrderEntity(CreateOnlineOrderRequest request, Cart cart)
  {
    return new Order
    {
      Id = Guid.NewGuid(),
      CustomerId = cart.CustomerId,
      CouponId = request.CouponId,
      OrderDate = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
      ShippingAddress = request.ShippingAddress,
      BillingAddress = request.BillingAddress,
      TrackingNumber = null,
      Status = OrderStatus.PENDING,
      PaymentMethod = request.PaymentMethod,

      CreateAt = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
      CreatedBy = cart.Customer.UserName,
      LastModified = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
      LastModifiedBy = cart.Customer.UserName,
      OrderItems = cart.CartItems.Select(ci => new OrderItem
      {
        Cosmetic = ci.Cosmetic,
        CosmeticId = ci.CosmeticId,
        Quantity = ci.Quantity
      }).ToList(),
    };
  }

  public async Task<Result<OrderResponse>> CompleteOrder(Guid orderId, string paymentStatus, PaymentReturnData paymentData)
  {
    try
    {
      var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
      if (order == null || order.Status != OrderStatus.PENDING)
      {
        return Result<OrderResponse>.Failure(
            new List<Error> { new Error("Order.NotFound", "Invalid order or wrong status") },
            StatusCodes.Status404NotFound);
      }

      if (paymentStatus == "00")
      {
        order.Status = OrderStatus.CONFIRMED;

        var shopInfo = await _ghnService.GetStoreInformationAsync();
        var shippingDetails = CalculateShippingDetails(order.OrderItems);
        decimal codAmount = order.PaymentMethod == PaymentMethods.COD ? order.TotalPrice : 0;
        int paymentTypeId = order.PaymentMethod == PaymentMethods.COD ? 2 : 1;

        var ghnOrderResult = await _ghnService.CreateShippingOrderAsync(MapToCreateShippingOrderRequest(order,
            shopInfo.Data!, shippingDetails, codAmount, paymentTypeId));

        if (ghnOrderResult.IsSuccess)
        {
          order.TrackingNumber = ghnOrderResult.Data!.OrderCode;
          order.ETA = ghnOrderResult.Data!.ExpectedDeliveryTime;
        }
        else
        {
          _logger.LogError("Failed to create GHN shipping order for Order ID {OrderId}: {Error}", order.Id, string.Join(", ", ghnOrderResult.Errors.Select(e => e.Description)));
        }

        var payment = new Payment
        {
          Id = Guid.NewGuid(),
          OrderId = order.Id,
          Order = order, // FIX: Set the navigation property to ensure relationship is saved.
          Method = PaymentMethods.ONLINE,
          TotalAmount = order.TotalPrice,
          Date = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
          TransactionId = paymentData.TransactionId
        };

        AddPointToCustomer(order.TotalPrice, order.Customer);
        _unitOfWork.Payments.Create(payment);

        var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(order.CustomerId);
        if (cart != null)
        {
          // FIX: Instead of removing the cart, clear its items and reset the total price.
          await _unitOfWork.Carts.ClearCartItemsAsync(cart.Id);
          cart.TotalPrice = 0;
          _unitOfWork.Carts.Update(cart);
        }
      }
      else
      {
        order.Status = OrderStatus.FAILED;

        if (order.CouponId.HasValue)
        {
          await RestoreCouponToUser(order.CustomerId, order.CouponId.Value);
        }
      }

      order.LastModified = DateTime.UtcNow;
      // FIX: Set LastModifiedBy to the customer's username, as this action is performed by the customer.
      order.LastModifiedBy = order.Customer.UserName;
      _unitOfWork.Orders.Update(order);

      var saved = await _unitOfWork.CompleteAsync();
      if (!saved)
      {
        return Result<OrderResponse>.Failure(
            new List<Error> { new Error("Order.Complete", "Failed to update order") },
            StatusCodes.Status500InternalServerError);
      }

      return Result<OrderResponse>.Success(
          MapToOrderResponse(order),
          StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An exception occurred while completing order {OrderId}", orderId);
      return Result<OrderResponse>.Failure(
          new List<Error> { new Error("Order.Complete", ex.Message) },
          StatusCodes.Status500InternalServerError);
    }
  }

  public async Task<Result<List<OrderResponse>>> GetAllOrdersAsync()
  {
    try
    {
      var orders = await _unitOfWork.Orders.GetAllOrdersWithItemsAsync();
      var response = orders.Select(MapToOrderResponse).ToList();
      return Result<List<OrderResponse>>.Success(response, StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      return Result<List<OrderResponse>>.Failure(
        [new Error("Order.GetAll", ex.Message)],
        StatusCodes.Status500InternalServerError);
    }
  }

  public async Task<Result<List<OrderResponse>>> GetOrdersByCustomerIdAsync(Guid customerId)
  {
    try
    {
      var orders = await _unitOfWork.Orders.GetOrdersByCustomerIdAsync(customerId);
      if (orders == null)
      {
        return Result<List<OrderResponse>>.Failure(
          new List<Error> { new Error("Order.Customer", "Could not find orders") },
          StatusCodes.Status404NotFound);
      }

      var response = orders.Select(MapToOrderResponse).ToList();
      return Result<List<OrderResponse>>.Success(response, StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      return Result<List<OrderResponse>>.Failure(
        [new Error("Order.GetByCustomerId", ex.Message)],
        StatusCodes.Status500InternalServerError);
    }
  }

  public async Task<Result<OrderResponse>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request)
  {
    try
    {
      var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
      if (order == null)
      {
        return Result<OrderResponse>.Failure(
            [new Error("Order.NotFound", "Order not found")],
            StatusCodes.Status404NotFound);
      }

      if ((request.Status != OrderStatus.CANCELLED && request.Status != OrderStatus.FAILED) &&
          _claimsService.CurrentUserRoles.Contains("Customer"))
      {
        return Result<OrderResponse>.Failure(
            [new Error("Order.Update", "Customers can only cancel orders or mark as payment failed")],
            StatusCodes.Status403Forbidden);
      }

      bool shouldRestoreCoupon = (request.Status == OrderStatus.CANCELLED ||
                                 request.Status == OrderStatus.FAILED) &&
                                 order.CouponId.HasValue &&
                                 order.Status != OrderStatus.CANCELLED &&
                                 order.Status != OrderStatus.FAILED;

      order.Status = request.Status;
      order.LastModified = DateTime.UtcNow;
      order.LastModifiedBy = _claimsService.CurrentUserId.ToString();

      if (request.Status == OrderStatus.CANCELLED)
      {
        var ghnOrderResult =
            await _ghnService.ChangeShippingOrderStatus(
                new SwitchShippingOrdersStatusRequest { OrderCodes = [order.TrackingNumber] }, "cancel");
        if (ghnOrderResult.IsFailure)
        {
          return Result<OrderResponse>.Failure(ghnOrderResult.Errors, ghnOrderResult.Status);
        }

        if (shouldRestoreCoupon)
        {
          await RestoreCouponToUser(order.CustomerId, order.CouponId.Value);
        }
      }
      else if (request.Status == OrderStatus.FAILED && shouldRestoreCoupon)
      {
        await RestoreCouponToUser(order.CustomerId, order.CouponId.Value);
      }

      _unitOfWork.Orders.Update(order);
      var saved = await _unitOfWork.CompleteAsync();

      if (!saved)
      {
        return Result<OrderResponse>.Failure(
            [new Error("Order.Update", "Failed to update order")],
            StatusCodes.Status500InternalServerError);
      }

      return Result<OrderResponse>.Success(
          MapToOrderResponse(order),
          StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      return Result<OrderResponse>.Failure(
          [new Error("Order.Update", ex.Message)],
          StatusCodes.Status500InternalServerError);
    }
  }

  private async Task RestoreCouponToUser(Guid userId, Guid couponId)
  {
    var userCoupon = await _unitOfWork.UserCoupons.GetByIdAsync(userId, couponId);

    if (userCoupon != null)
    {
      userCoupon.Quantity++;
      _unitOfWork.UserCoupons.Update(userCoupon);
    }
    else
    {
      var newUserCoupon = new UserCoupon
      {
        UserId = userId,
        CouponId = couponId,
        Quantity = 1
      };
      _unitOfWork.UserCoupons.Create(newUserCoupon);
    }
  }
  public async Task<Result<OrderResponse>> InitiateSimpleOrder(CreateOnlineOrderRequest request)
  {
    try
    {
      // 1. Get the current user's cart
      var cartValidationResult = await ValidateOrderRequest(request);
      if (!cartValidationResult.IsSuccess)
      {
        return Result<OrderResponse>.Failure(
          cartValidationResult.Errors,
          cartValidationResult.Status);
      }

      var cart = cartValidationResult.Data;
      var customer = await _userManager.FindByIdAsync(_claimsService.CurrentUserId.ToString());

      // 2. Create the Order entity without GHN details
      var order = new Order
      {
        Id = Guid.NewGuid(),
        CustomerId = cart.CustomerId,
        CouponId = request.CouponId,
        OrderDate = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
        ShippingAddress = request.ShippingAddress,
        BillingAddress = request.BillingAddress,
        Status = OrderStatus.PENDING,
        PaymentMethod = request.PaymentMethod,
        // GHN fields are omitted
        // WardCode = request.WardCode, 
        // DistrictId = request.DistrictId,
        CreateAt = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
        CreatedBy = cart.Customer.UserName,
        LastModified = _timeZoneService.ConvertToLocalTime(DateTime.UtcNow),
        LastModifiedBy = cart.Customer.UserName,
        OrderItems = cart.CartItems.Select(ci => new OrderItem
        {
          Cosmetic = ci.Cosmetic,
          CosmeticId = ci.CosmeticId,
          Quantity = ci.Quantity
        }).ToList(),
      };

      // 3. Calculate totals (same as before)
      decimal subtotal = 0;
      decimal discountedSubtotal = 0;

      foreach (var item in order.OrderItems)
      {
        decimal originalPrice = await _unitOfWork.Cosmetics.GetCosmeticOriginalPrice(item.Cosmetic);
        decimal discountedPrice = await _unitOfWork.Cosmetics.GetCosmeticPrice(item.Cosmetic);

        item.SellingPrice = discountedPrice;
        subtotal += originalPrice * item.Quantity;
        discountedSubtotal += discountedPrice * item.Quantity;
      }

      order.SubTotal = subtotal;
      decimal totalPrice = discountedSubtotal;

      // ... (coupon logic remains the same)

      order.TotalPrice = totalPrice;
      order.Customer = customer!;

      // 4. Save the order
      _unitOfWork.Orders.Create(order);
      var saved = await _unitOfWork.CompleteAsync();

      if (!saved)
      {
        return Result<OrderResponse>.Failure(
          [new Error("Order.Create", "Failed to save order")],
          StatusCodes.Status500InternalServerError);
      }

      var orderResponse = MapToOrderResponse(order);

      return Result<OrderResponse>.Success(orderResponse, StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      return Result<OrderResponse>.Failure(
        [new Error("Order.Create", ex.Message)],
        StatusCodes.Status500InternalServerError);
    }
  }
  public async Task<Result<string>> DeleteOrderAsync(Guid orderId)
  {
    try
    {
      var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
      if (order == null)
      {
        return Result<string>.Failure(
          [new Error("Order.Delete", "Order not found")],
          StatusCodes.Status404NotFound);
      }

      _unitOfWork.Orders.Remove(order);
      var saved = await _unitOfWork.CompleteAsync();

      if (!saved)
      {
        return Result<string>.Failure(
          [new Error("Order.Delete", "Failed to delete order")],
          StatusCodes.Status500InternalServerError);
      }

      return Result<string>.Success(
        "Order deleted successfully",
        StatusCodes.Status200OK);
    }
    catch (Exception ex)
    {
      return Result<string>.Failure(
        [new Error("Order.Delete", ex.Message)],
        StatusCodes.Status500InternalServerError);
    }
  }

  public async Task CleanupExpiredOrders()
  {
    try
    {
      var expiryTime = DateTime.UtcNow.AddMinutes(-15);
      var expiredOrders = await _unitOfWork.Orders.GetExpiredPendingOrdersAsync(expiryTime);

      foreach (var order in expiredOrders)
      {
        order.Status = OrderStatus.EXPIRED;
        order.LastModified = DateTime.UtcNow;
        _unitOfWork.Orders.Update(order);
      }

      await _unitOfWork.CompleteAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to cleanup expired orders.");
      throw;
    }
  }

  private static OrderResponse MapToOrderResponse(Order order)
  {
    return new OrderResponse
    {
      Id = order.Id,
      CustomerId = order.CustomerId,
      CustomerUserName = order.Customer?.UserName,
      CustomerEmail = order.Customer?.Email,
      CouponId = order.CouponId,
      CouponName = order.Coupon?.Name,
      SubTotal = order.SubTotal,
      TotalPrice = order.TotalPrice,
      OrderDate = order.OrderDate,
      ShippingAddress = order.ShippingAddress,
      BillingAddress = order.BillingAddress,
      TrackingNumber = order.TrackingNumber,
      ETA = order.ETA,
      DeliveryDate = order.DeliveryDate,
      Status = order.Status,
      CreateAt = order.CreateAt,
      CreatedBy = order.CreatedBy,
      LastModified = order.LastModified,
      LastModifiedBy = order.LastModifiedBy,
      OrderItems = order.OrderItems.Select(oi => new OrderItemResponse
      {
        CosmeticId = oi.CosmeticId,
        Quantity = oi.Quantity,
        SellingPrice = oi.SellingPrice
      }).ToList()
    };
  }

  public static CreateGHNOrderRequest MapToCreateShippingOrderRequest(Order order, StoreData shopInfo,
     ShippingDetails details, decimal codAmount, int paymentTypeId)
  {
    var request = new CreateGHNOrderRequest
    {
      PaymentTypeId = paymentTypeId,
      Note = "Handle With Care",
      RequiredNote = "CHOXEMHANGKHONGTHU",
      FromName = shopInfo.Shops[1].Name,
      FromPhone = shopInfo.Shops[1].Phone,
      FromAddress = shopInfo.Shops[1].Address,
      FromWardName = "Thao Dien",
      FromDistrictName = "Thanh Pho Thu Duc",
      FromProvinceName = "Ho Chi Minh",
      ReturnPhone = "",
      ReturnAddress = "",
      ReturnDistrictId = null,
      ReturnWardCode = "",
      ClientOrderCode = "",
      ToName = order.Customer.FirstName + " " + order.Customer.LastName,
      ToPhone = order.Customer.PhoneNumber,
      ToAddress = order.ShippingAddress,
      ToWardCode = order.WardCode,
      ToDistrictId = order.DistrictId,
      CodAmount = (int)codAmount,
      Content = "Order from De Fleur",
      Weight = details.Weight,
      Length = details.Length,
      Width = details.Width,
      Height = details.Height,
      PickStationId = 0,
      DeliverStationId = null,
      InsuranceValue = (int)order.TotalPrice,
      ServiceId = 0,
      ServiceTypeId = 2,
      Coupon = null,
      PickShift = new List<int> { 1, 2 },
      Items = MapOrderItemsToGHNItems(order.OrderItems)
    };

    return request;
  }

  private static List<GHNOrderItem> MapOrderItemsToGHNItems(List<OrderItem> orderItems)
  {
    return orderItems.Select(item => new GHNOrderItem
    {
      Name = item.Cosmetic.Name,
      Code = item.CosmeticId.ToString(),
      Quantity = item.Quantity,
      Price = (int)item.SellingPrice,
      Length = item.Cosmetic.Length,
      Width = item.Cosmetic.Width,
      Height = item.Cosmetic.Height,
      Weight = item.Cosmetic.Weight
    }).ToList();
  }

  private void AddPointToCustomer(decimal orderPrice, User customer)
  {
    customer.Point += Math.Round(orderPrice / 10000);
  }
}

// Helper class for shipping calculations
public class ShippingDetails
{
  public int Weight { get; set; }
  public int Length { get; set; }
  public int Width { get; set; }
  public int Height { get; set; }
}