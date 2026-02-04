using CarRental.Application.DTOs.Car;
using FluentValidation;

namespace CarRental.Application.Validators;

/// <summary>
/// Validator for CreateCarRequest.
/// </summary>
public class CreateCarValidator : AbstractValidator<CreateCarRequest>
{
    public CreateCarValidator()
    {
        RuleFor(x => x.Make)
            .NotEmpty().WithMessage("Make is required.")
            .MaximumLength(50).WithMessage("Make cannot exceed 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(50).WithMessage("Model cannot exceed 50 characters.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.Now.Year + 1)
            .WithMessage($"Year must be between 1900 and {DateTime.Now.Year + 1}.");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("License plate is required.")
            .MaximumLength(20).WithMessage("License plate cannot exceed 20 characters.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required.")
            .MaximumLength(30).WithMessage("Color cannot exceed 30 characters.");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative.");

        RuleFor(x => x.DailyRate)
            .GreaterThan(0).WithMessage("Daily rate must be greater than zero.");

        RuleFor(x => x.SeatingCapacity)
            .InclusiveBetween(1, 15).WithMessage("Seating capacity must be between 1 and 15.");

        RuleFor(x => x.FuelType)
            .NotEmpty().WithMessage("Fuel type is required.");

        RuleFor(x => x.Transmission)
            .NotEmpty().WithMessage("Transmission is required.");
    }
}
