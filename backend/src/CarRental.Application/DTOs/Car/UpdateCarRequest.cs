using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CarRental.Domain.Enums;

namespace CarRental.Application.DTOs.Car;

/// <summary>
/// DTO for updating an existing car.
/// </summary>
public class UpdateCarRequest
{
    /// <summary>Car manufacturer</summary>
    /// <example>Toyota</example>
    [DefaultValue("Toyota")]
    [Required]
    public string Make { get; set; } = string.Empty;

    /// <summary>Car model</summary>
    /// <example>Camry</example>
    [DefaultValue("Camry")]
    [Required]
    public string Model { get; set; } = string.Empty;

    /// <summary>Manufacturing year</summary>
    /// <example>2024</example>
    [DefaultValue(2024)]
    [Required]
    public int Year { get; set; }

    /// <summary>License plate number</summary>
    /// <example>ABC-1234</example>
    [DefaultValue("ABC-1234")]
    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>Car color</summary>
    /// <example>Silver</example>
    [DefaultValue("Silver")]
    public string Color { get; set; } = string.Empty;

    /// <summary>Current mileage</summary>
    /// <example>15000</example>
    [DefaultValue(15000)]
    public int Mileage { get; set; }

    /// <summary>Daily rental rate in USD</summary>
    /// <example>75.00</example>
    [DefaultValue(75.00)]
    [Required]
    public decimal DailyRate { get; set; }

    /// <summary>Car status (0=Available, 1=Rented, 2=Maintenance, 3=Reserved, 4=OutOfService)</summary>
    /// <example>0</example>
    public CarStatus Status { get; set; }

    /// <summary>Car category (0=Economy, 1=Compact, 2=Midsize, 3=FullSize, 4=Luxury, 5=SUV, 6=Van, 7=Truck, 8=Convertible, 9=Sports)</summary>
    /// <example>2</example>
    public CarCategory Category { get; set; }

    /// <summary>URL to car image</summary>
    /// <example>https://example.com/cars/toyota-camry.jpg</example>
    public string? ImageUrl { get; set; }

    /// <summary>Car description</summary>
    /// <example>Comfortable sedan with excellent fuel economy</example>
    public string? Description { get; set; }

    /// <summary>Number of seats</summary>
    /// <example>5</example>
    [DefaultValue(5)]
    public int SeatingCapacity { get; set; }

    /// <summary>Type of fuel</summary>
    /// <example>Gasoline</example>
    [DefaultValue("Gasoline")]
    public string FuelType { get; set; } = string.Empty;

    /// <summary>Transmission type</summary>
    /// <example>Automatic</example>
    [DefaultValue("Automatic")]
    public string Transmission { get; set; } = string.Empty;

    /// <summary>Has air conditioning</summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool HasAirConditioning { get; set; }

    /// <summary>Has GPS navigation</summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool HasGPS { get; set; }
}
