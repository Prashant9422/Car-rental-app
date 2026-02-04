using CarRental.Application.DTOs.Car;
using CarRental.Application.Interfaces;
using CarRental.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// API endpoints for car management.
/// </summary>
public class CarsController : BaseApiController
{
    private readonly ICarService _carService;
    private readonly ILogger<CarsController> _logger;

    public CarsController(ICarService carService, ILogger<CarsController> logger)
    {
        _carService = carService;
        _logger = logger;
    }

    /// <summary>
    /// Get all cars.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CarDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all cars");
        var result = await _carService.GetAllAsync(cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get a car by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting car with ID: {CarId}", id);
        var result = await _carService.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Get available cars for a date range.
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CarDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting available cars from {StartDate} to {EndDate}", startDate, endDate);
        var result = await _carService.GetAvailableCarsAsync(startDate, endDate, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Search cars with filters.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CarDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string? make,
        [FromQuery] string? model,
        [FromQuery] CarCategory? category,
        [FromQuery] decimal? maxDailyRate,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching cars with filters");
        var result = await _carService.SearchCarsAsync(make, model, category, maxDailyRate, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new car.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateCarRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new car: {Make} {Model}", request.Make, request.Model);
        var result = await _carService.CreateAsync(request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Update an existing car.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCarRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating car with ID: {CarId}", id);
        var result = await _carService.UpdateAsync(id, request, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete a car.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting car with ID: {CarId}", id);
        var result = await _carService.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Update car status.
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Manager,Staff")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] CarStatus status, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating status for car {CarId} to {Status}", id, status);
        var result = await _carService.UpdateStatusAsync(id, status, cancellationToken);
        return HandleResult(result);
    }
}
