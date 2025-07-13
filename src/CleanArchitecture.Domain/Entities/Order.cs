// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Domain/Entities/Order.cs
public class Order : Entity<Guid>
{
  public Guid CustomerId { get; set; }
  public User Customer { get; set; } = default!;
  public Guid? CouponId { get; set; }
  public Coupon? Coupon { get; set; }  // made nullable if coupon is optional
  public decimal SubTotal { get; set; }
  public decimal TotalPrice { get; set; }
  public DateTime OrderDate { get; set; }
  public string ShippingAddress { get; set; } = default!;
  public string BillingAddress { get; set; } = default!;
  public string? TrackingNumber { get; set; }
  public DateTime? ETA { get; set; } // made nullable for walk in orders
  public DateTime? DeliveryDate { get; set; } // made nullable if delivery date is set later
  public string Status { get; set; } = default!;
  public string PaymentMethod { get; set; } = default!;
  public int DistrictId { get; set; }   //  propertY to store GHN address details
  public string WardCode { get; set; } = string.Empty;
  public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
  public List<Refund> Refunds { get; set; } = new List<Refund>();
}