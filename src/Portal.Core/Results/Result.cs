namespace Portal.Core.Results;

/// <summary>
/// Standard API response structure following Success/Failure pattern
/// </summary>
/// <typeparam name="T">Type of data returned in successful response</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static Result<T> Success(T data, string? message = null)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message ?? "Operation successful"
        };
    }

    public static Result<T> Failure(string message, List<string>? errors = null)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static Result<T> Failure(string message, string error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}

/// <summary>
/// Result without data payload
/// </summary>
public class Result
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static Result Success(string? message = null)
    {
        return new Result
        {
            IsSuccess = true,
            Message = message ?? "Operation successful"
        };
    }

    public static Result Failure(string message, List<string>? errors = null)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static Result Failure(string message, string error)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}
