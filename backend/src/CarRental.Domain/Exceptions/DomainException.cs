namespace CarRental.Domain.Exceptions;

/// <summary>
/// Base exception for all domain-specific exceptions.
/// </summary>
public class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string message, string code = "DOMAIN_ERROR") 
        : base(message)
    {
        Code = code;
    }

    public DomainException(string message, Exception innerException, string code = "DOMAIN_ERROR") 
        : base(message, innerException)
    {
        Code = code;
    }
}
