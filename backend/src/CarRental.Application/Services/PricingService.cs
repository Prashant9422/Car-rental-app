using CarRental.Application.Common;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Microsoft.Extensions.Options;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of pricing service with configurable rates.
/// </summary>
public class PricingService : IPricingService
{
    private readonly RentalSettings _settings;

    public PricingService(IOptions<RentalSettings> settings)
    {
        _settings = settings.Value;
    }

    public PricingResult CalculateRentalCost(Car car, DateTime startDate, DateTime endDate)
    {
        var totalDays = (endDate - startDate).Days + 1;
        var baseCost = totalDays * car.DailyRate;

        // Apply duration-based discounts
        decimal discountPercentage = 0;
        string? discountType = null;

        if (totalDays >= 30)
        {
            discountPercentage = _settings.MonthlyDiscountPercentage;
            discountType = "Monthly";
        }
        else if (totalDays >= 7)
        {
            discountPercentage = _settings.WeeklyDiscountPercentage;
            discountType = "Weekly";
        }

        var discountAmount = baseCost * (discountPercentage / 100);
        var totalCost = baseCost - discountAmount;

        return new PricingResult
        {
            DailyRate = car.DailyRate,
            TotalDays = totalDays,
            BaseCost = baseCost,
            DiscountPercentage = discountPercentage,
            DiscountAmount = discountAmount,
            TotalCost = totalCost,
            DiscountType = discountType
        };
    }

    public decimal CalculateDeposit(decimal rentalCost)
    {
        return rentalCost * (_settings.DepositPercentage / 100);
    }

    public decimal CalculateLateFee(DateTime expectedReturnDate, DateTime actualReturnDate)
    {
        if (actualReturnDate <= expectedReturnDate)
            return 0;

        var lateDays = (actualReturnDate - expectedReturnDate).Days;
        return lateDays * _settings.LateFeePerDay;
    }

    public decimal CalculateCancellationFee(decimal rentalCost, DateTime startDate, DateTime cancellationDate)
    {
        var hoursUntilStart = (startDate - cancellationDate).TotalHours;

        // Free cancellation within the grace period
        if (hoursUntilStart >= _settings.FreeCancellationHours)
            return 0;

        return rentalCost * (_settings.CancellationFeePercentage / 100);
    }
}
