// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Presentation/Endpoints/PaymentController.cs
using CleanArchitecture.Application.DTOs.VnPay;
using CleanArchitecture.Application.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CleanArchitecture.Presentation.Endpoints
{
  public class PaymentController : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      var group = app.MapGroup("api/payment").WithTags("Payment Management");

      #region Create Payment URL Endpoint
      group.MapPost("/create-payment-url", (IVnPayIntegrationService vnPayIntegrationService, HttpContext context, VnPayPaymentRequestDto request) =>
      {
        var result = vnPayIntegrationService.CreatePaymentUrl(request, context);
        return result.Match(Message.SUCCESSFUL_CREATED("Payment URL"));
      })
      .WithName("CreatePaymentUrl")
      .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .WithSummary("Create Payment URL")
      .WithDescription("Creates a payment URL for VNPay integration.");
      #endregion

      #region Process VNPay Return Endpoint
      group.MapGet("/vnpay-return", async (IVnPayIntegrationService vnPayIntegrationService, IOrderService orderService, HttpContext context) =>
      {
        var vnPayResult = await vnPayIntegrationService.ProcessReturnAsync(context.Request.Query);

        if (vnPayResult.IsFailure)
        {
          // Redirect to a frontend failure page using a custom app scheme.
          return Results.Redirect($"defleur-app://payment/failure?message={vnPayResult.Errors.First().Description}");
        }

        var vnPayResponse = vnPayResult.Data!;
        var orderInfo = vnPayResponse.OrderDescription!;

        var searchString = "De Fleur - Payment for order id ";
        int startIndex = orderInfo.IndexOf(searchString);
        string orderIdStr = orderInfo.Substring(startIndex + searchString.Length);

        if (!Guid.TryParse(orderIdStr, out var orderId))
        {
          return Results.Redirect($"defleur-app://payment/failure?message=InvalidOrderId");
        }

        var paymentData = new PaymentReturnData
        {
          TransactionId = vnPayResponse.TransactionId,
          TotalAmount = vnPayResponse.TotalAmount,
          ResponseCode = vnPayResponse.ResponseCode
        };

        var orderResult = await orderService.CompleteOrder(orderId, vnPayResponse.ResponseCode ?? string.Empty, paymentData);

        if (orderResult.IsSuccess)
        {
          // Redirect to a frontend success page with the order ID using a custom app scheme.
          return Results.Redirect($"defleur-app://payment/success?orderId={orderId}");
        }

        // Redirect to a frontend failure page if order completion fails.
        return Results.Redirect($"defleur-app://payment/failure?message={orderResult.Errors.First().Description}");
      })
      .WithName("ProcessVnPayReturn")
      .Produces(StatusCodes.Status302Found)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .WithSummary("Process VNPay Return")
      .WithDescription("Processes the VNPay return response, validates the signature, and updates the order status.");
      #endregion
    }
  }
}