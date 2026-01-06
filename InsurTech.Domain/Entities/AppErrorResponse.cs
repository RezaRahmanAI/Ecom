using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurTech.Domain.Entities;

public class AppErrorResponse
{
    public bool Success { get; set; } = false;
    public int StatusCode { get; set; }
    public string Message { get; set; } = "Something went wrong";
    public string? Detail { get; set; } // dev environment এ helpful
    public string TraceId { get; set; } = default!;
    public object? Errors { get; set; }
}
