using CarRental.Domain.Entities;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Interfaces;

/// <summary>
/// Repository interface for Car-specific operations.
/// </summary>
public interface ICarRepository : IRepository<Car>
{
    Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Car>> GetCarsByCategoryAsync(CarCategory category, CancellationToken cancellationToken = default);
    Task<Car?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Car>> SearchCarsAsync(string? make, string? model, CarCategory? category, decimal? maxDailyRate, CancellationToken cancellationToken = default);
}
