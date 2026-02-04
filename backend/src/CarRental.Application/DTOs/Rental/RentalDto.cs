namespace CarRental.Application.DTOs.Rental;

/// <summary>
/// DTO for rental data returned in responses.
/// </summary>
public class RentalDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public string CarInfo { get; set; } = string.Empty; // e.g., "2023 Toyota Camry"
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public int RentalDays { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? LateFee { get; set; }
    public decimal? DamageCharges { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PickupLocation { get; set; }
    public string? ReturnLocation { get; set; }
    public int? OdometerAtPickup { get; set; }
    public int? OdometerAtReturn { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
