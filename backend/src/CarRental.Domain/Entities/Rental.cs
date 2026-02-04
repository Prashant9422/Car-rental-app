using CarRental.Domain.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

/// <summary>
/// Represents a car rental transaction.
/// </summary>
public class Rental : BaseEntity
{
    public Guid CarId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? LateFee { get; set; }
    public decimal? DamageCharges { get; set; }
    public RentalStatus Status { get; set; } = RentalStatus.Pending;
    
    /// <summary>Customer's pickup address or location where they want to collect the car</summary>
    public string? PickupLocation { get; set; }
    
    /// <summary>Location where customer will return the car</summary>
    public string? ReturnLocation { get; set; }
    
    public int? OdometerAtPickup { get; set; }
    public int? OdometerAtReturn { get; set; }
    public string? Notes { get; set; }
    
    // Deposit handling
    public decimal? DepositAmount { get; set; }
    public bool DepositReturned { get; set; } = false;

    // Navigation properties
    public virtual Car Car { get; set; } = null!;
    public virtual User Customer { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    /// <summary>
    /// Calculates the number of rental days.
    /// </summary>
    public int RentalDays => (EndDate - StartDate).Days + 1;

    /// <summary>
    /// Calculates the base rental cost.
    /// </summary>
    public decimal CalculateBaseCost()
    {
        return RentalDays * DailyRate;
    }

    /// <summary>
    /// Calculates late fees if applicable.
    /// </summary>
    public decimal CalculateLateFee(decimal lateFeePerDay)
    {
        if (ActualReturnDate == null || ActualReturnDate <= EndDate)
            return 0;

        var lateDays = (ActualReturnDate.Value - EndDate).Days;
        return lateDays * lateFeePerDay;
    }
}

