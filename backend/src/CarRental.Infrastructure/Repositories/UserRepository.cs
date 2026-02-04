using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories;

/// <summary>
/// User repository implementation.
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(CarRentalDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Role == UserRole.Customer && u.IsActive)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync(cancellationToken);
    }
}
