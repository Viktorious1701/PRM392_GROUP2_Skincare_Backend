// PRM392_GROUP2_Skincare_Backend/src/CleanArchitecture.Application/DTOs/VnPay/VnPayPaymentRequestDto.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.VnPay
{
  public class VnPayPaymentRequestDto
  {
    public Guid OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
  }
}