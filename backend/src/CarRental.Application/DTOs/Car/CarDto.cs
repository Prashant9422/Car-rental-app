using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Car;

/// <summary>
/// DTO for car data returned in responses.
/// </summary>
public class CarDto
{
    public Guid Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public decimal DailyRate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public int SeatingCapacity { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public string Transmission { get; set; } = string.Empty;
    public bool HasAirConditioning { get; set; }
    public bool HasGPS { get; set; }
    public DateTime CreatedAt { get; set; }
}
