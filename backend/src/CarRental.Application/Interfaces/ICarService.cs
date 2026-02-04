using CarRental.Application.Common;
using CarRental.Application.DTOs.Car;
using CarRental.Domain.Enums;

namespace CarRental.Application.Interfaces;

/// <summary>
/// Service interface for car operations.
/// </summary>
public interface ICarService
{
    Task<Result<CarDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CarDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CarDto>>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CarDto>>> SearchCarsAsync(string? make, string? model, CarCategory? category, decimal? maxDailyRate, CancellationToken cancellationToken = default);
    Task<Result<CarDto>> CreateAsync(CreateCarRequest request, CancellationToken cancellationToken = default);
    Task<Result<CarDto>> UpdateAsync(Guid id, UpdateCarRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateStatusAsync(Guid id, CarStatus status, CancellationToken cancellationToken = default);
}
