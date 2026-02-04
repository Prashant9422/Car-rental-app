using CarRental.Application.Common;
using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of car service with business logic.
/// </summary>
public class CarService : ICarService
{
    private readonly IUnitOfWork _unitOfWork;

    public CarService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CarDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car == null)
            return Result<CarDto>.NotFound($"Car with ID '{id}' was not found.");

        return Result<CarDto>.Success(MapToDto(car));
    }

    public async Task<Result<IEnumerable<CarDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cars = await _unitOfWork.Cars.GetAllAsync(cancellationToken);
        return Result<IEnumerable<CarDto>>.Success(cars.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<CarDto>>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        if (endDate <= startDate)
            return Result<IEnumerable<CarDto>>.Failure("End date must be after start date.", "VALIDATION_ERROR");

        var cars = await _unitOfWork.Cars.GetAvailableCarsAsync(startDate, endDate, cancellationToken);
        return Result<IEnumerable<CarDto>>.Success(cars.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<CarDto>>> SearchCarsAsync(string? make, string? model, CarCategory? category, decimal? maxDailyRate, CancellationToken cancellationToken = default)
    {
        var cars = await _unitOfWork.Cars.SearchCarsAsync(make, model, category, maxDailyRate, cancellationToken);
        return Result<IEnumerable<CarDto>>.Success(cars.Select(MapToDto));
    }

    public async Task<Result<CarDto>> CreateAsync(CreateCarRequest request, CancellationToken cancellationToken = default)
    {
        // Check for duplicate license plate
        var existingCar = await _unitOfWork.Cars.GetByLicensePlateAsync(request.LicensePlate, cancellationToken);
        if (existingCar != null)
            return Result<CarDto>.Failure($"A car with license plate '{request.LicensePlate}' already exists.", "CONFLICT");

        var car = new Car
        {
            Id = Guid.NewGuid(),
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            LicensePlate = request.LicensePlate,
            Color = request.Color,
            Mileage = request.Mileage,
            DailyRate = request.DailyRate,
            Status = CarStatus.Available,
            Category = request.Category,
            ImageUrl = request.ImageUrl,
            Description = request.Description,
            SeatingCapacity = request.SeatingCapacity,
            FuelType = request.FuelType,
            Transmission = request.Transmission,
            HasAirConditioning = request.HasAirConditioning,
            HasGPS = request.HasGPS,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Cars.AddAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CarDto>.Success(MapToDto(car));
    }

    public async Task<Result<CarDto>> UpdateAsync(Guid id, UpdateCarRequest request, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car == null)
            return Result<CarDto>.NotFound($"Car with ID '{id}' was not found.");

        // Check for duplicate license plate (excluding current car)
        if (car.LicensePlate != request.LicensePlate)
        {
            var existingCar = await _unitOfWork.Cars.GetByLicensePlateAsync(request.LicensePlate, cancellationToken);
            if (existingCar != null)
                return Result<CarDto>.Failure($"A car with license plate '{request.LicensePlate}' already exists.", "CONFLICT");
        }

        car.Make = request.Make;
        car.Model = request.Model;
        car.Year = request.Year;
        car.LicensePlate = request.LicensePlate;
        car.Color = request.Color;
        car.Mileage = request.Mileage;
        car.DailyRate = request.DailyRate;
        car.Status = request.Status;
        car.Category = request.Category;
        car.ImageUrl = request.ImageUrl;
        car.Description = request.Description;
        car.SeatingCapacity = request.SeatingCapacity;
        car.FuelType = request.FuelType;
        car.Transmission = request.Transmission;
        car.HasAirConditioning = request.HasAirConditioning;
        car.HasGPS = request.HasGPS;
        car.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Cars.Update(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CarDto>.Success(MapToDto(car));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car == null)
            return Result.Failure($"Car with ID '{id}' was not found.", "NOT_FOUND");

        // Check if car has active rentals
        var activeRentals = await _unitOfWork.Rentals.GetRentalsByCarAsync(id, cancellationToken);
        if (activeRentals.Any(r => r.Status == RentalStatus.Active || r.Status == RentalStatus.Confirmed))
            return Result.Failure("Cannot delete a car with active rentals.", "BUSINESS_RULE_VIOLATION");

        _unitOfWork.Cars.Remove(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateStatusAsync(Guid id, CarStatus status, CancellationToken cancellationToken = default)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id, cancellationToken);
        if (car == null)
            return Result.Failure($"Car with ID '{id}' was not found.", "NOT_FOUND");

        car.Status = status;
        car.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Cars.Update(car);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static CarDto MapToDto(Car car)
    {
        return new CarDto
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            LicensePlate = car.LicensePlate,
            Color = car.Color,
            Mileage = car.Mileage,
            DailyRate = car.DailyRate,
            Status = car.Status.ToString(),
            Category = car.Category.ToString(),
            ImageUrl = car.ImageUrl,
            Description = car.Description,
            SeatingCapacity = car.SeatingCapacity,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            HasAirConditioning = car.HasAirConditioning,
            HasGPS = car.HasGPS,
            CreatedAt = car.CreatedAt
        };
    }
}
