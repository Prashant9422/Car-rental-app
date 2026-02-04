namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the current status of a car in the fleet.
/// </summary>
public enum CarStatus
{
    Available = 0,
    Rented = 1,
    Maintenance = 2,
    Reserved = 3,
    OutOfService = 4
}
