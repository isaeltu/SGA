namespace SGA.Api.Models;

public sealed record ApiErrorResponse(
    string ErrorId,
    int Status,
    string Code,
    string Message,
    string Path,
    DateTimeOffset Timestamp,
    IDictionary<string, string[]>? Errors = null);
