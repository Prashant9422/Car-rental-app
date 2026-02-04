using CarRental.Application.DTOs.Rental;
using FluentValidation;

namespace CarRental.Application.Validators;

/// <summary>
/// Validator for CreateRentalRequest.
/// </summary>
public class CreateRentalValidator : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty().WithMessage("Car ID is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.PickupLocation)
            .MaximumLength(200).WithMessage("Pickup location cannot exceed 200 characters.");

        RuleFor(x => x.ReturnLocation)
            .MaximumLength(200).WithMessage("Return location cannot exceed 200 characters.");
    }
}
