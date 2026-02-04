using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces;

/// <summary>
/// Repository interface for User-specific operations.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
}
