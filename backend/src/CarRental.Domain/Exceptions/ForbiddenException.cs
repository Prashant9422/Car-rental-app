namespace CarRental.Domain.Exceptions;

/// <summary>
/// Exception thrown when authorization fails.
/// </summary>
public class ForbiddenException : DomainException
{
    public ForbiddenException(string message = "You do not have permission to perform this action.")
        : base(message, "FORBIDDEN")
    {
    }

    public static ForbiddenException InsufficientRole(string requiredRole)
    {
        return new ForbiddenException($"This action requires '{requiredRole}' role.");
    }

    public static ForbiddenException ResourceAccessDenied()
    {
        return new ForbiddenException("You do not have access to this resource.");
    }
}
