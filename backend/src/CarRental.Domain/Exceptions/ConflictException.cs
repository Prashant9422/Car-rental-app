namespace CarRental.Domain.Exceptions;

/// <summary>
/// Exception thrown when a conflict occurs (e.g., duplicate resource).
/// </summary>
public class ConflictException : DomainException
{
    public ConflictException(string message)
        : base(message, "CONFLICT")
    {
    }

    public static ConflictException DuplicateResource(string resourceType, string field, object value)
    {
        return new ConflictException($"{resourceType} with {field} '{value}' already exists.");
    }
}
