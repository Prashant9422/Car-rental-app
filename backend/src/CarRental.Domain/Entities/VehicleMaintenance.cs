using CarRental.Domain.Common;

namespace CarRental.Domain.Entities;

/// <summary>
/// Represents a vehicle maintenance record.
/// </summary>
public class VehicleMaintenance : BaseEntity
{
    public Guid CarId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;  // Oil Change, Tire Rotation, Inspection, etc.
    public decimal Cost { get; set; }
    public int? OdometerReading { get; set; }
    public DateTime? NextServiceDue { get; set; }
    public string? ServiceProvider { get; set; }
    public string? Notes { get; set; }

    // Navigation property
    public virtual Car Car { get; set; } = null!;
}
