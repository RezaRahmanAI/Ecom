namespace InsurTech.Domain.Entities;

public class AppSuccessResponse<T>
{
    public bool Success { get; set; } = true;
    public int StatusCode { get; set; }
    public string Message { get; set; } = "Request successful";
    public string TraceId { get; set; } = default!;
    public T? Data { get; set; }
}
