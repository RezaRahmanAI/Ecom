using InsurTech.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InsurTech.Api.Filters;

public class ApiResponseWrapperFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not null)
        {
            return;
        }

        switch (context.Result)
        {
            case ObjectResult objectResult:
                WrapObjectResult(context, objectResult);
                break;
            case StatusCodeResult statusCodeResult:
                WrapStatusCodeResult(context, statusCodeResult);
                break;
        }
    }

    private static void WrapObjectResult(ActionExecutedContext context, ObjectResult objectResult)
    {
        if (objectResult.Value is AppErrorResponse || IsSuccessResponse(objectResult.Value))
        {
            return;
        }

        if (objectResult.Value is null)
        {
            WrapStatusCodeResult(context, new StatusCodeResult(objectResult.StatusCode ?? StatusCodes.Status200OK));
            return;
        }

        var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
        if (statusCode >= StatusCodes.Status400BadRequest)
        {
            var errorResponse = new AppErrorResponse
            {
                StatusCode = statusCode,
                Message = GetErrorMessage(statusCode),
                TraceId = context.HttpContext.TraceIdentifier,
                Errors = objectResult.Value
            };

            objectResult.Value = errorResponse;
            return;
        }

        var response = new AppSuccessResponse<object>
        {
            StatusCode = statusCode,
            Message = "Request successful",
            TraceId = context.HttpContext.TraceIdentifier,
            Data = objectResult.Value
        };

        objectResult.Value = response;
    }

    private static void WrapStatusCodeResult(ActionExecutedContext context, StatusCodeResult statusCodeResult)
    {
        var statusCode = statusCodeResult.StatusCode;
        if (statusCode >= StatusCodes.Status400BadRequest)
        {
            context.Result = new ObjectResult(new AppErrorResponse
            {
                StatusCode = statusCode,
                Message = GetErrorMessage(statusCode),
                TraceId = context.HttpContext.TraceIdentifier
            })
            {
                StatusCode = statusCode
            };

            return;
        }

        context.Result = new ObjectResult(new AppSuccessResponse<object?>
        {
            StatusCode = statusCode,
            Message = "Request successful",
            TraceId = context.HttpContext.TraceIdentifier,
            Data = null
        })
        {
            StatusCode = statusCode
        };
    }

    private static string GetErrorMessage(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad request.",
            StatusCodes.Status401Unauthorized => "Unauthorized.",
            StatusCodes.Status403Forbidden => "Forbidden.",
            StatusCodes.Status404NotFound => "Resource not found.",
            StatusCodes.Status409Conflict => "Conflict.",
            _ => "Something went wrong."
        };
    }

    private static bool IsSuccessResponse(object value)
    {
        var type = value.GetType();
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AppSuccessResponse<>);
    }
}
