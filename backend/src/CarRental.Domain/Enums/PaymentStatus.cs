namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the status of a payment.
/// </summary>
public enum PaymentStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4,
    PartialRefund = 5
}
