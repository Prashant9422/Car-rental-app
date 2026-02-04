using CarRental.Domain.Common;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

/// <summary>
/// Represents a car in the rental fleet.
/// </summary>
public class Car : BaseEntity
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public decimal DailyRate { get; set; }
    public CarStatus Status { get; set; } = CarStatus.Available;
    public CarCategory Category { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int SeatingCapacity { get; set; } = 5;
    public string FuelType { get; set; } = "Gasoline";
    public string Transmission { get; set; } = "Automatic";
    public bool HasAirConditioning { get; set; } = true;
    public bool HasGPS { get; set; } = false;

    // Maintenance tracking
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }

    // Navigation properties
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public virtual ICollection<VehicleMaintenance> MaintenanceRecords { get; set; } = new List<VehicleMaintenance>();
}
