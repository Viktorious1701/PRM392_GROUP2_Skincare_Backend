// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/DTOs/OrderDto/CreateOnlineOrderRequest.cs
namespace CleanArchitecture.Application.DTOs.OrderDto;

public class CreateOnlineOrderRequest : ICreateOrderRequest
{
  public Guid? CouponId { get; set; }
  public string ShippingAddress { get; set; }
  public string BillingAddress { get; set; }
  public string PaymentMethod { get; set; }
  public string Currency { get; set; }
  // Logic: Removed GHN-specific fields to simplify the DTO for now.
  // public string WardCode { get; set; }
  // public int DistrictId { get; set; }
}