namespace SGA.Api.Services;

public sealed record ErrorLogEntry(
    string ErrorId,
    DateTimeOffset Timestamp,
    int Status,
    string ErrorCode,
    string Message,
    string? StackTrace,
    string Path,
    string Method,
    string? User,
    string? Ip,
    string TraceIdentifier,
    IDictionary<string, string[]>? ValidationErrors);
