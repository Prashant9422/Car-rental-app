using CarRental.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// Base controller with common functionality.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Converts a Result to an appropriate ActionResult.
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Data = result.Value,
                Message = "Operation completed successfully."
            });
        }

        return result.ErrorCode switch
        {
            "NOT_FOUND" => NotFound(new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "Resource not found."
            }),
            "VALIDATION_ERROR" => BadRequest(new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "Validation failed.",
                Errors = result.ValidationErrors
            }),
            "CONFLICT" => Conflict(new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "Resource conflict."
            }),
            "UNAUTHORIZED" => Unauthorized(new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "Unauthorized."
            }),
            "FORBIDDEN" => StatusCode(403, new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "Forbidden."
            }),
            "BUSINESS_RULE_VIOLATION" => UnprocessableEntity(new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "Business rule violation."
            }),
            _ => BadRequest(new ApiResponse<T>
            {
                Success = false,
                Message = result.Error ?? "An error occurred."
            })
        };
    }

    /// <summary>
    /// Converts a Result (without value) to an appropriate ActionResult.
    /// </summary>
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Operation completed successfully."
            });
        }

        return result.ErrorCode switch
        {
            "NOT_FOUND" => NotFound(new ApiResponse { Success = false, Message = result.Error ?? "Resource not found." }),
            "VALIDATION_ERROR" => BadRequest(new ApiResponse { Success = false, Message = result.Error ?? "Validation failed." }),
            "CONFLICT" => Conflict(new ApiResponse { Success = false, Message = result.Error ?? "Resource conflict." }),
            "BUSINESS_RULE_VIOLATION" => UnprocessableEntity(new ApiResponse { Success = false, Message = result.Error ?? "Business rule violation." }),
            _ => BadRequest(new ApiResponse { Success = false, Message = result.Error ?? "An error occurred." })
        };
    }
}

/// <summary>
/// Standard API response wrapper.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; set; }
}

/// <summary>
/// Standard API response wrapper without data.
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
