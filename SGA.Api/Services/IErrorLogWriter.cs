namespace SGA.Api.Services;

public interface IErrorLogWriter
{
    Task WriteAsync(ErrorLogEntry entry, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ErrorLogEntry>> QueryAsync(
        DateTimeOffset? from,
        DateTimeOffset? to,
        string? user,
        string? errorCode,
        CancellationToken cancellationToken);
}
