namespace CarRental.Domain.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated.
/// </summary>
public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message)
        : base(message, "BUSINESS_RULE_VIOLATION")
    {
    }

    public static BusinessRuleException CarNotAvailable(Guid carId)
    {
        return new BusinessRuleException($"Car with ID '{carId}' is not available for rental.");
    }

    public static BusinessRuleException RentalAlreadyCompleted(Guid rentalId)
    {
        return new BusinessRuleException($"Rental with ID '{rentalId}' has already been completed.");
    }

    public static BusinessRuleException InvalidDateRange()
    {
        return new BusinessRuleException("End date must be after start date.");
    }
}
