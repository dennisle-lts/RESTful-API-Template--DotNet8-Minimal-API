using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var problemDetails = CreateProblemDetails(exception, httpContext);

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode =
            problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        var json = JsonSerializer.Serialize(
            problemDetails,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        await httpContext.Response.WriteAsync(json, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(Exception exception, HttpContext context)
    {
        var instance = context.Request.GetDisplayUrl();

        switch (exception)
        {
            case UnauthorizedAccessException unauthorizedEx:
                _logger.LogInformation(
                    unauthorizedEx,
                    "Unauthorized access attempt at {Path}",
                    instance
                );
                return CreateUnauthorizedErrorResponse(unauthorizedEx, instance);

            case ArgumentException argumentEx:
                _logger.LogInformation(argumentEx, "Resource not found at {Path}", instance);
                return CreateNotFoundErrorResponse(argumentEx, instance);

            case InvalidOperationException businessEx:
                _logger.LogWarning(businessEx, "Business logic error at {Path}", instance);
                return CreateBusinessLogicErrorResponse(businessEx, instance);

            default:
                _logger.LogError(exception, "Unexpected error at {Path}", instance);
                return CreateInternalServerErrorResponse(exception, instance);
        }
    }

    private static ProblemDetails CreateUnauthorizedErrorResponse(
        UnauthorizedAccessException exception,
        string instance
    )
    {
        return new ProblemDetails
        {
            Type = "https://example.com/problems/unauthorized",
            Title = "Unauthorized",
            Status = (int)HttpStatusCode.Unauthorized,
            Detail = exception.Message,
            Instance = instance,
        };
    }

    private static ProblemDetails CreateNotFoundErrorResponse(
        ArgumentException exception,
        string instance
    )
    {
        return new ProblemDetails
        {
            Type = "https://example.com/problems/not-found",
            Title = "Not Found",
            Status = (int)HttpStatusCode.NotFound,
            Detail = exception.Message,
            Instance = instance,
        };
    }

    private static ProblemDetails CreateBusinessLogicErrorResponse(
        InvalidOperationException exception,
        string instance
    )
    {
        return new ProblemDetails
        {
            Type = "https://example.com/problems/business-logic-error",
            Title = "Business Logic Error",
            Status = (int)HttpStatusCode.BadRequest,
            Detail = exception.Message,
            Instance = instance,
        };
    }

    private static ProblemDetails CreateInternalServerErrorResponse(
        Exception exception,
        string instance
    )
    {
        return new ProblemDetails
        {
            Type = "https://example.com/problems/internal-error",
            Title = "Internal Server Error",
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = "An internal server error occurred",
            Instance = instance,
        };
    }
}
