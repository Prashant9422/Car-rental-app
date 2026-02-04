using System.Net;
using System.Text.Json;
using CarRental.Domain.Exceptions;

namespace CarRental.API.Middleware;

/// <summary>
/// Global exception handling middleware that transforms exceptions into API responses.
/// </summary>
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorCode, message, errors) = exception switch
        {
            NotFoundException ex => (HttpStatusCode.NotFound, ex.Code, ex.Message, null),
            ValidationException ex => (HttpStatusCode.BadRequest, ex.Code, ex.Message, ex.Errors),
            ConflictException ex => (HttpStatusCode.Conflict, ex.Code, ex.Message, null),
            BusinessRuleException ex => (HttpStatusCode.UnprocessableEntity, ex.Code, ex.Message, null),
            UnauthorizedException ex => (HttpStatusCode.Unauthorized, ex.Code, ex.Message, null),
            ForbiddenException ex => (HttpStatusCode.Forbidden, ex.Code, ex.Message, null),
            DomainException ex => (HttpStatusCode.BadRequest, ex.Code, ex.Message, null),
            _ => (HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred.", null as IDictionary<string, string[]>)
        };

        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Success = false,
            StatusCode = (int)statusCode,
            ErrorCode = errorCode,
            Message = message,
            Errors = errors,
            TraceId = context.TraceIdentifier,
            Timestamp = DateTime.UtcNow
        };

        // Include stack trace in development
        if (_env.IsDevelopment() && statusCode == HttpStatusCode.InternalServerError)
        {
            response.Detail = exception.StackTrace;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}

/// <summary>
/// Standard error response structure.
/// </summary>
public class ErrorResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public IDictionary<string, string[]>? Errors { get; set; }
    public string TraceId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Detail { get; set; }
}

/// <summary>
/// Extension method for adding the middleware.
/// </summary>
public static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandler>();
    }
}
