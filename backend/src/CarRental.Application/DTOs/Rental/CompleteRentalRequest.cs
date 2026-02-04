using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.DTOs.Rental;

/// <summary>
/// DTO for completing/returning a rental.
/// </summary>
public class CompleteRentalRequest
{
    /// <summary>Actual date and time the car was returned</summary>
    /// <example>2026-02-07T14:30:00</example>
    [Required]
    public DateTime ActualReturnDate { get; set; }

    /// <summary>Odometer reading at return (optional)</summary>
    /// <example>15350</example>
    [DefaultValue(15350)]
    public int? OdometerAtReturn { get; set; }

    /// <summary>Amount to charge for any vehicle damage (optional)</summary>
    /// <example>0.00</example>
    [DefaultValue(0.00)]
    public decimal? DamageCharges { get; set; }

    /// <summary>Additional notes about the return condition</summary>
    /// <example>Vehicle returned in good condition, minor exterior dust</example>
    [DefaultValue("Vehicle returned in good condition")]
    public string? Notes { get; set; }
}
