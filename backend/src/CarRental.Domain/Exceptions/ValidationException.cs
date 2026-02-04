namespace CarRental.Domain.Exceptions;

/// <summary>
/// Exception thrown when validation fails.
/// </summary>
public class ValidationException : DomainException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.", "VALIDATION_ERROR")
    {
        Errors = errors;
    }

    public ValidationException(string field, string message)
        : base($"Validation failed for {field}: {message}", "VALIDATION_ERROR")
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { message } }
        };
    }
}
