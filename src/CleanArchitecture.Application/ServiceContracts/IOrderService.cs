using CleanArchitecture.Application.DTOs.Order;
using CleanArchitecture.Application.DTOs.OrderDto;
using CleanArchitecture.Application.DTOs.VnPay;

namespace CleanArchitecture.Application.ServiceContracts
{
  public interface IOrderService
  {
    Task<Result<List<OrderResponse>>> GetAllOrdersAsync();
    Task<Result<List<OrderResponse>>> GetOrdersByCustomerIdAsync(Guid customerId);
    Task<Result<OrderResponse>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request);
    Task<Result<string>> DeleteOrderAsync(Guid orderId);
    Task<Result<OrderResponse>> InitiateOrder(CreateOnlineOrderRequest request);
    Task<Result<OrderResponse>> InitiateOrder(CreateWalkInOrderRequest request);
    Task<Result<OrderResponse>> CompleteOrder(Guid orderId, string paymentStatus, PaymentReturnData paymentData);
    Task<Result<OrderResponse>> InitiateSimpleOrder(CreateOnlineOrderRequest request);
    Task CleanupExpiredOrders();

  }
}
