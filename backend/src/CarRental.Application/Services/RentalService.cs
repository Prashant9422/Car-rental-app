using CarRental.Application.Common;
using CarRental.Application.DTOs.Rental;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Domain.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of rental service with business logic.
/// </summary>
public class RentalService : IRentalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPricingService _pricingService;

    public RentalService(IUnitOfWork unitOfWork, IPricingService pricingService)
    {
        _unitOfWork = unitOfWork;
        _pricingService = pricingService;
    }

    public async Task<Result<RentalDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rental = await _unitOfWork.Rentals.GetRentalWithDetailsAsync(id, cancellationToken);
        if (rental == null)
            return Result<RentalDto>.NotFound($"Rental with ID '{id}' was not found.");

        return Result<RentalDto>.Success(MapToDto(rental));
    }

    public async Task<Result<IEnumerable<RentalDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rentals = await _unitOfWork.Rentals.GetAllAsync(cancellationToken);
        return Result<IEnumerable<RentalDto>>.Success(rentals.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<RentalDto>>> GetByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var rentals = await _unitOfWork.Rentals.GetRentalsByCustomerAsync(customerId, cancellationToken);
        return Result<IEnumerable<RentalDto>>.Success(rentals.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<RentalDto>>> GetByStatusAsync(RentalStatus status, CancellationToken cancellationToken = default)
    {
        var rentals = await _unitOfWork.Rentals.GetRentalsByStatusAsync(status, cancellationToken);
        return Result<IEnumerable<RentalDto>>.Success(rentals.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<RentalDto>>> GetActiveRentalsAsync(CancellationToken cancellationToken = default)
    {
        var rentals = await _unitOfWork.Rentals.GetActiveRentalsAsync(cancellationToken);
        return Result<IEnumerable<RentalDto>>.Success(rentals.Select(MapToDto));
    }

    public async Task<Result<IEnumerable<RentalDto>>> GetOverdueRentalsAsync(CancellationToken cancellationToken = default)
    {
        var rentals = await _unitOfWork.Rentals.GetOverdueRentalsAsync(cancellationToken);
        return Result<IEnumerable<RentalDto>>.Success(rentals.Select(MapToDto));
    }

    public async Task<Result<RentalDto>> CreateAsync(CreateRentalRequest request, CancellationToken cancellationToken = default)
    {
        // Validate car exists and is available
        var car = await _unitOfWork.Cars.GetByIdAsync(request.CarId, cancellationToken);
        if (car == null)
            return Result<RentalDto>.NotFound($"Car with ID '{request.CarId}' was not found.");

        if (car.Status != CarStatus.Available)
            return Result<RentalDto>.Failure("This car is not available for rental.", "BUSINESS_RULE_VIOLATION");

        // Validate customer exists
        var customer = await _unitOfWork.Users.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            return Result<RentalDto>.NotFound($"Customer with ID '{request.CustomerId}' was not found.");

        // Check for overlapping rentals
        var hasOverlap = await _unitOfWork.Rentals.HasOverlappingRentalAsync(
            request.CarId, request.StartDate, request.EndDate, null, cancellationToken);
        if (hasOverlap)
            return Result<RentalDto>.Failure("This car has an overlapping rental for the selected dates.", "CONFLICT");

        // Calculate pricing with duration-based discounts
        var pricingResult = _pricingService.CalculateRentalCost(car, request.StartDate, request.EndDate);
        var depositAmount = _pricingService.CalculateDeposit(pricingResult.TotalCost);

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            CarId = request.CarId,
            CustomerId = request.CustomerId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DailyRate = pricingResult.DailyRate,
            TotalCost = pricingResult.TotalCost,
            DepositAmount = depositAmount,
            Status = RentalStatus.Pending,
            PickupLocation = request.PickupLocation,
            ReturnLocation = request.ReturnLocation,
            Notes = pricingResult.DiscountType != null 
                ? $"{pricingResult.DiscountType} discount ({pricingResult.DiscountPercentage}%) applied. {request.Notes}" 
                : request.Notes,
            CreatedAt = DateTime.UtcNow,
            Car = car,
            Customer = customer
        };

        await _unitOfWork.Rentals.AddAsync(rental, cancellationToken);
        
        // Reserve the car
        car.Status = CarStatus.Reserved;
        _unitOfWork.Cars.Update(car);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RentalDto>.Success(MapToDto(rental));
    }

    public async Task<Result<RentalDto>> ConfirmAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rental = await _unitOfWork.Rentals.GetRentalWithDetailsAsync(id, cancellationToken);
        if (rental == null)
            return Result<RentalDto>.NotFound($"Rental with ID '{id}' was not found.");

        if (rental.Status != RentalStatus.Pending)
            return Result<RentalDto>.Failure("Only pending rentals can be confirmed.", "BUSINESS_RULE_VIOLATION");

        rental.Status = RentalStatus.Confirmed;
        rental.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Rentals.Update(rental);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RentalDto>.Success(MapToDto(rental));
    }

    public async Task<Result<RentalDto>> StartAsync(Guid id, int? odometerAtPickup, CancellationToken cancellationToken = default)
    {
        var rental = await _unitOfWork.Rentals.GetRentalWithDetailsAsync(id, cancellationToken);
        if (rental == null)
            return Result<RentalDto>.NotFound($"Rental with ID '{id}' was not found.");

        if (rental.Status != RentalStatus.Confirmed)
            return Result<RentalDto>.Failure("Only confirmed rentals can be started.", "BUSINESS_RULE_VIOLATION");

        rental.Status = RentalStatus.Active;
        rental.OdometerAtPickup = odometerAtPickup;
        rental.UpdatedAt = DateTime.UtcNow;

        // Mark car as rented
        rental.Car.Status = CarStatus.Rented;
        _unitOfWork.Cars.Update(rental.Car);

        _unitOfWork.Rentals.Update(rental);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RentalDto>.Success(MapToDto(rental));
    }

    public async Task<Result<RentalDto>> CompleteAsync(Guid id, CompleteRentalRequest request, CancellationToken cancellationToken = default)
    {
        var rental = await _unitOfWork.Rentals.GetRentalWithDetailsAsync(id, cancellationToken);
        if (rental == null)
            return Result<RentalDto>.NotFound($"Rental with ID '{id}' was not found.");

        if (rental.Status != RentalStatus.Active)
            return Result<RentalDto>.Failure("Only active rentals can be completed.", "BUSINESS_RULE_VIOLATION");

        rental.Status = RentalStatus.Completed;
        rental.ActualReturnDate = request.ActualReturnDate;
        rental.OdometerAtReturn = request.OdometerAtReturn;
        rental.DamageCharges = request.DamageCharges;
        rental.Notes = request.Notes ?? rental.Notes;
        rental.UpdatedAt = DateTime.UtcNow;

        // Calculate late fee using pricing service if applicable
        var lateFee = _pricingService.CalculateLateFee(rental.EndDate, request.ActualReturnDate);
        if (lateFee > 0)
        {
            rental.LateFee = lateFee;
            rental.TotalCost += lateFee;
        }

        // Add damage charges to total
        if (request.DamageCharges.HasValue && request.DamageCharges.Value > 0)
        {
            rental.TotalCost += request.DamageCharges.Value;
        }

        // Handle deposit return (return deposit if no damage charges or late fees)
        rental.DepositReturned = (rental.LateFee ?? 0) == 0 && (rental.DamageCharges ?? 0) == 0;

        // Mark car as available
        rental.Car.Status = CarStatus.Available;
        if (request.OdometerAtReturn.HasValue)
        {
            rental.Car.Mileage = request.OdometerAtReturn.Value;
        }
        _unitOfWork.Cars.Update(rental.Car);

        _unitOfWork.Rentals.Update(rental);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RentalDto>.Success(MapToDto(rental));
    }

    public async Task<Result<RentalDto>> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rental = await _unitOfWork.Rentals.GetRentalWithDetailsAsync(id, cancellationToken);
        if (rental == null)
            return Result<RentalDto>.NotFound($"Rental with ID '{id}' was not found.");

        if (rental.Status == RentalStatus.Completed || rental.Status == RentalStatus.Cancelled)
            return Result<RentalDto>.Failure("Cannot cancel a completed or already cancelled rental.", "BUSINESS_RULE_VIOLATION");

        rental.Status = RentalStatus.Cancelled;
        rental.UpdatedAt = DateTime.UtcNow;

        // Make car available again
        rental.Car.Status = CarStatus.Available;
        _unitOfWork.Cars.Update(rental.Car);

        _unitOfWork.Rentals.Update(rental);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RentalDto>.Success(MapToDto(rental));
    }

    private static RentalDto MapToDto(Rental rental)
    {
        return new RentalDto
        {
            Id = rental.Id,
            CarId = rental.CarId,
            CarInfo = rental.Car != null ? $"{rental.Car.Year} {rental.Car.Make} {rental.Car.Model}" : string.Empty,
            CustomerId = rental.CustomerId,
            CustomerName = rental.Customer?.FullName ?? string.Empty,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            ActualReturnDate = rental.ActualReturnDate,
            RentalDays = rental.RentalDays,
            DailyRate = rental.DailyRate,
            TotalCost = rental.TotalCost,
            LateFee = rental.LateFee,
            DamageCharges = rental.DamageCharges,
            Status = rental.Status.ToString(),
            PickupLocation = rental.PickupLocation,
            ReturnLocation = rental.ReturnLocation,
            OdometerAtPickup = rental.OdometerAtPickup,
            OdometerAtReturn = rental.OdometerAtReturn,
            Notes = rental.Notes,
            CreatedAt = rental.CreatedAt
        };
    }
}
