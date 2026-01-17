using InsurTech.Application.Common;

public class Result
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = "";
    public ErrorType? ErrorType { get; init; }
    public Dictionary<string, string[]>? Errors { get; init; }

    public static Result Success(string message = "OK")
        => new() { IsSuccess = true, Message = message };

    public static Result Failure(string message, ErrorType type = ErrorType.Failure)
        => new() { IsSuccess = false, Message = message, ErrorType = type };

    public static Result NotFound(string message = "Not found")
        => Failure(message, ErrorType.NotFound);

    public static Result Conflict(string message = "Conflict")
        => Failure(message, ErrorType.Conflict);

    public static Result Validation(Dictionary<string, string[]> errors, string message = "Validation failed")
        => new() { IsSuccess = false, Message = message, ErrorType = ErrorType.Validation, Errors = errors };
}
