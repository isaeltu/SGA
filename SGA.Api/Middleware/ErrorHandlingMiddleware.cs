using System.Net;
using System.Text.Json;
using FluentValidation;
using SGA.Api.Models;
using SGA.Api.Services;

namespace SGA.Api.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IErrorLogWriter errorLogWriter)
    {
        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            await HandleAsync(context, exception, errorLogWriter).ConfigureAwait(false);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception exception, IErrorLogWriter errorLogWriter)
    {
        var (status, code, message, errors) = MapException(exception);
        var errorId = Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
        var timestamp = DateTimeOffset.UtcNow;

        logger.LogError(
            exception,
            "API error {ErrorId} {Status} on {Method} {Path} by {User}",
            errorId,
            (int)status,
            context.Request.Method,
            context.Request.Path,
            context.User.Identity?.Name ?? "anonymous");

        var logEntry = new ErrorLogEntry(
            errorId,
            timestamp,
            (int)status,
            code,
            message,
            exception.ToString(),
            context.Request.Path,
            context.Request.Method,
            context.User.Identity?.Name,
            context.Connection.RemoteIpAddress?.ToString(),
            context.TraceIdentifier,
            errors);

        await errorLogWriter.WriteAsync(logEntry, context.RequestAborted).ConfigureAwait(false);

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse(
            errorId,
            (int)status,
            code,
            message,
            context.Request.Path,
            timestamp,
            errors);

        var payload = JsonSerializer.Serialize(response, JsonOptions);
        await context.Response.WriteAsync(payload, context.RequestAborted).ConfigureAwait(false);
    }

    private static (HttpStatusCode Status, string Code, string Message, IDictionary<string, string[]>? Errors) MapException(Exception exception)
        => exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                "validation_error",
                "Existen errores de validacion en la solicitud.",
                validationException.Errors
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.PropertyName) ? "general" : x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(e => e.ErrorMessage).Distinct().ToArray(),
                        StringComparer.OrdinalIgnoreCase)),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "unauthorized", "La sesion no es valida o ha expirado.", null),
            KeyNotFoundException => (HttpStatusCode.NotFound, "not_found", "No se encontro el recurso solicitado.", null),
            TimeoutException => (HttpStatusCode.RequestTimeout, "timeout", "La operacion excedio el tiempo permitido.", null),
            InvalidOperationException invalidOperationException when invalidOperationException.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                => (HttpStatusCode.NotFound, "not_found", invalidOperationException.Message, null),
            InvalidOperationException invalidOperationException when invalidOperationException.Message.Contains("permission", StringComparison.OrdinalIgnoreCase)
                || invalidOperationException.Message.Contains("forbidden", StringComparison.OrdinalIgnoreCase)
                => (HttpStatusCode.Forbidden, "forbidden", invalidOperationException.Message, null),
            _ => (HttpStatusCode.InternalServerError, "server_error", "Ha ocurrido un error interno. Intenta nuevamente mas tarde.", null)
        };
}
