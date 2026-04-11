using System.Text.Json;

namespace SGA.Api.Services;

public sealed class FileErrorLogWriter : IErrorLogWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    private readonly SemaphoreSlim gate = new(1, 1);
    private readonly string logDirectory;
    private readonly ILogger<FileErrorLogWriter> logger;

    public FileErrorLogWriter(IHostEnvironment hostEnvironment, ILogger<FileErrorLogWriter> logger)
    {
        this.logger = logger;
        logDirectory = Path.Combine(hostEnvironment.ContentRootPath, "logs");
        Directory.CreateDirectory(logDirectory);
    }

    public async Task WriteAsync(ErrorLogEntry entry, CancellationToken cancellationToken)
    {
        var filePath = BuildFilePath(entry.Timestamp);
        var line = JsonSerializer.Serialize(entry, JsonOptions);

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await File.AppendAllTextAsync(filePath, line + Environment.NewLine, cancellationToken).ConfigureAwait(false);
            CleanupOldFiles(DateTimeOffset.UtcNow.AddDays(-30));
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task<IReadOnlyCollection<ErrorLogEntry>> QueryAsync(
        DateTimeOffset? from,
        DateTimeOffset? to,
        string? user,
        string? errorCode,
        CancellationToken cancellationToken)
    {
        var result = new List<ErrorLogEntry>();
        var files = Directory.EnumerateFiles(logDirectory, "errors-*.jsonl", SearchOption.TopDirectoryOnly)
            .OrderByDescending(path => path)
            .ToList();

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var lines = await File.ReadAllLinesAsync(file, cancellationToken).ConfigureAwait(false);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    var entry = JsonSerializer.Deserialize<ErrorLogEntry>(line, JsonOptions);
                    if (entry is null)
                    {
                        continue;
                    }

                    if (from.HasValue && entry.Timestamp < from.Value)
                    {
                        continue;
                    }

                    if (to.HasValue && entry.Timestamp > to.Value)
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(user) &&
                        !string.Equals(entry.User, user.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(errorCode) &&
                        !string.Equals(entry.ErrorCode, errorCode.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    result.Add(entry);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "No se pudo deserializar una linea del archivo de logs {FilePath}", file);
                }
            }
        }

        return result.OrderByDescending(x => x.Timestamp).ToList();
    }

    private string BuildFilePath(DateTimeOffset timestamp)
        => Path.Combine(logDirectory, $"errors-{timestamp:yyyy-MM-dd}.jsonl");

    private void CleanupOldFiles(DateTimeOffset threshold)
    {
        var files = Directory.EnumerateFiles(logDirectory, "errors-*.jsonl", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            try
            {
                var creation = File.GetCreationTimeUtc(file);
                if (creation < threshold.UtcDateTime)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "No se pudo limpiar archivo de log antiguo {FilePath}", file);
            }
        }
    }
}
