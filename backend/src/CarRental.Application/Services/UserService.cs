using CarRental.Application.Common;
using CarRental.Application.DTOs.User;
using CarRental.Application.Interfaces;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of user service.
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return Result<UserDto>.NotFound($"User with ID '{id}' was not found.");

        return Result<UserDto>.Success(MapToDto(user));
    }

    public async Task<Result<UserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email.ToLower(), cancellationToken);
        if (user == null)
            return Result<UserDto>.NotFound($"User with email '{email}' was not found.");

        return Result<UserDto>.Success(MapToDto(user));
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.Users.GetActiveCustomersAsync(cancellationToken);
        return Result<IEnumerable<UserDto>>.Success(users.Select(MapToDto));
    }

    public async Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return Result<UserDto>.NotFound($"User with ID '{id}' was not found.");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.Street = request.Street;
        user.City = request.City;
        user.State = request.State;
        user.PostalCode = request.PostalCode;
        user.Country = request.Country;
        user.DriversLicenseNumber = request.DriversLicenseNumber;
        user.DriversLicenseExpiry = request.DriversLicenseExpiry;
        user.DateOfBirth = request.DateOfBirth;
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserDto>.Success(MapToDto(user));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return Result.Failure($"User with ID '{id}' was not found.", "NOT_FOUND");

        // Check if user has active rentals
        var activeRentals = await _unitOfWork.Rentals.GetRentalsByCustomerAsync(id, cancellationToken);
        if (activeRentals.Any(r => r.Status == RentalStatus.Active || r.Status == RentalStatus.Confirmed))
            return Result.Failure("Cannot delete a user with active rentals.", "BUSINESS_RULE_VIOLATION");

        _unitOfWork.Users.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return Result.Failure($"User with ID '{id}' was not found.", "NOT_FOUND");

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
        if (user == null)
            return Result.Failure($"User with ID '{id}' was not found.", "NOT_FOUND");

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static UserDto MapToDto(Domain.Entities.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            Street = user.Street,
            City = user.City,
            State = user.State,
            PostalCode = user.PostalCode,
            Country = user.Country,
            DriversLicenseNumber = user.DriversLicenseNumber,
            DriversLicenseExpiry = user.DriversLicenseExpiry,
            DateOfBirth = user.DateOfBirth,
            CreatedAt = user.CreatedAt
        };
    }
}
