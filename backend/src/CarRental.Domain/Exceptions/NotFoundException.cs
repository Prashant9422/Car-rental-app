namespace CarRental.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : DomainException
{
    public string ResourceType { get; }
    public object? ResourceId { get; }

    public NotFoundException(string resourceType, object? resourceId = null)
        : base($"{resourceType} was not found.", "NOT_FOUND")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }

    public static NotFoundException For<T>(object? id = null) where T : class
    {
        return new NotFoundException(typeof(T).Name, id);
    }
}
