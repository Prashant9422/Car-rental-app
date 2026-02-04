using CarRental.Application.Common;
using CarRental.Application.DTOs.User;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for user operations.
/// </summary>
public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
    Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
}
