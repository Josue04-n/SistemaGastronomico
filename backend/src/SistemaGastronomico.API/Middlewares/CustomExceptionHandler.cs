namespace SistemaGastronomico.API.Middlewares;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Application.Common.Exceptions;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ocurrió un error no controlado: {Message}", exception.Message);

        var (statusCode, problemDetails) = exception switch
        {
            ValidationException validationEx => (
                StatusCodes.Status400BadRequest,
                new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de validación",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Detail = validationEx.Message,
                    Extensions = { ["errors"] = validationEx.Errors }
                }),
            NotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Recurso no encontrado",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Detail = notFoundEx.Message
                }),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "No autorizado",
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                }),
            _ => (
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Detail = "Ocurrió un error inesperado al procesar la solicitud."
                })
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
