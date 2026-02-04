namespace CarRental.Application.Common;

/// <summary>
/// Represents the result of an operation that may succeed or fail.
/// Implements the Result pattern for better error handling.
/// </summary>
/// <typeparam name="T">The type of the value on success.</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? Error { get; }
    public string? ErrorCode { get; }
    public IDictionary<string, string[]>? ValidationErrors { get; }

    private Result(bool isSuccess, T? value, string? error, string? errorCode, IDictionary<string, string[]>? validationErrors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors;
    }

    public static Result<T> Success(T value) => new(true, value, null, null, null);
    
    public static Result<T> Failure(string error, string errorCode = "ERROR") 
        => new(false, default, error, errorCode, null);
    
    public static Result<T> ValidationFailure(IDictionary<string, string[]> errors) 
        => new(false, default, "Validation failed", "VALIDATION_ERROR", errors);

    public static Result<T> NotFound(string message = "Resource not found") 
        => new(false, default, message, "NOT_FOUND", null);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
}

/// <summary>
/// Represents the result of an operation that may succeed or fail (no return value).
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public string? ErrorCode { get; }

    private Result(bool isSuccess, string? error, string? errorCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true, null, null);
    public static Result Failure(string error, string errorCode = "ERROR") => new(false, error, errorCode);
}
