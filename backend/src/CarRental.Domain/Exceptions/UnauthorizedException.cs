namespace CarRental.Domain.Exceptions;

/// <summary>
/// Exception thrown when authentication fails.
/// </summary>
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "Authentication is required.")
        : base(message, "UNAUTHORIZED")
    {
    }

    public static UnauthorizedException InvalidCredentials()
    {
        return new UnauthorizedException("Invalid email or password.");
    }

    public static UnauthorizedException TokenExpired()
    {
        return new UnauthorizedException("Your session has expired. Please log in again.");
    }
}
