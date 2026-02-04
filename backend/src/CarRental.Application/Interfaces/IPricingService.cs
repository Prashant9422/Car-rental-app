using CarRental.Application.Common;
using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for pricing calculations.
/// </summary>
public interface IPricingService
{
    /// <summary>
    /// Calculates the total rental cost including any applicable discounts.
    /// </summary>
    PricingResult CalculateRentalCost(Car car, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Calculates the deposit amount based on rental cost.
    /// </summary>
    decimal CalculateDeposit(decimal rentalCost);

    /// <summary>
    /// Calculates late fees for a rental.
    /// </summary>
    decimal CalculateLateFee(DateTime expectedReturnDate, DateTime actualReturnDate);

    /// <summary>
    /// Calculates cancellation fee for a rental.
    /// </summary>
    decimal CalculateCancellationFee(decimal rentalCost, DateTime startDate, DateTime cancellationDate);
}

/// <summary>
/// Result of pricing calculation including breakdown.
/// </summary>
public class PricingResult
{
    public decimal DailyRate { get; set; }
    public int TotalDays { get; set; }
    public decimal BaseCost { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalCost { get; set; }
    public string? DiscountType { get; set; } // "Weekly", "Monthly", or null
}
