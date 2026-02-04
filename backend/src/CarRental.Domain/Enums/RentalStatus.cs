namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the current status of a rental.
/// </summary>
public enum RentalStatus
{
    Pending = 0,
    Confirmed = 1,
    Active = 2,
    Completed = 3,
    Cancelled = 4,
    Overdue = 5
}
