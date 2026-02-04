namespace CarRental.Application.Common;

/// <summary>
/// Configuration settings for rental operations.
/// </summary>
public class RentalSettings
{
    public const string SectionName = "RentalSettings";

    /// <summary>
    /// Late fee charged per day when car is returned late.
    /// </summary>
    public decimal LateFeePerDay { get; set; } = 50.00m;

    /// <summary>
    /// Deposit percentage of total rental cost.
    /// </summary>
    public decimal DepositPercentage { get; set; } = 20.00m;

    /// <summary>
    /// Minimum number of rental days allowed.
    /// </summary>
    public int MinRentalDays { get; set; } = 1;

    /// <summary>
    /// Maximum number of rental days allowed in a single booking.
    /// </summary>
    public int MaxRentalDays { get; set; } = 30;

    /// <summary>
    /// Cancellation fee as percentage of total rental cost.
    /// </summary>
    public decimal CancellationFeePercentage { get; set; } = 10.00m;

    /// <summary>
    /// Hours before pickup when cancellation is free.
    /// </summary>
    public int FreeCancellationHours { get; set; } = 24;

    /// <summary>
    /// Discount percentage for weekly rentals (7+ days).
    /// </summary>
    public decimal WeeklyDiscountPercentage { get; set; } = 10.00m;

    /// <summary>
    /// Discount percentage for monthly rentals (30+ days).
    /// </summary>
    public decimal MonthlyDiscountPercentage { get; set; } = 20.00m;
}
