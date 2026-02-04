using CarRental.Application.DTOs.Payment;
using FluentValidation;

namespace CarRental.Application.Validators;

/// <summary>
/// Validator for CreatePaymentRequest.
/// </summary>
public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.RentalId)
            .NotEmpty().WithMessage("Rental ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method.");
    }
}
