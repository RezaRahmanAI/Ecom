using InsurTech.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace InsurTech.Api;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
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
            // 1) Log full exception (সবসময়)
            _logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);


            //2. Map exception to status code and message
            var (statusCode, message, errors) = MapException(ex);

            //3. Prepare response
            var response = new AppErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message,
                Errors = errors,
                Detail = _env.IsDevelopment() ? ex.StackTrace : null,
                TraceId = context.TraceIdentifier,
            };


            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);


        }
    }



    private static (HttpStatusCode statusCode, string message, object? errors) MapException(Exception ex)
    {
        return ex switch
        {
            ArgumentException argEx => (HttpStatusCode.BadRequest, argEx.Message, null),
            KeyNotFoundException keyNotFoundEx => (HttpStatusCode.NotFound, keyNotFoundEx.Message, null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };
    }

}
