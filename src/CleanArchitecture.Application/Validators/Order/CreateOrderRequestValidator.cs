using CleanArchitecture.Application.DTOs.OrderDto;

namespace CleanArchitecture.Application.Validators.Order;

public class CreateOrderRequestValidator : AbstractValidator<CreateOnlineOrderRequest>
{
  public CreateOrderRequestValidator()
  {
    RuleFor(x => x.ShippingAddress)
        .NotEmpty().WithMessage("Shipping address is required.")
        .MaximumLength(255).WithMessage("Shipping address is too long.");

    RuleFor(x => x.BillingAddress)
        .NotEmpty().WithMessage("Billing address is required.")
        .MaximumLength(255).WithMessage("Billing address is too long.");

    RuleFor(x => x.PaymentMethod)
        .NotEmpty().WithMessage("Payment method is required.")
        .Must(pm => new[] { "ONLINE", "COD" }.Contains(pm.ToUpper()))
        .WithMessage("Invalid payment method.");

    RuleFor(x => x.Currency)
        .NotEmpty().WithMessage("Currency is required.")
        .Matches("^(VND|USD)$").WithMessage("Currency must be 'VND' or 'USD'.");

 
  }
}
