namespace SGA.Web.Services;

public sealed class ApiClientException : Exception
{
    public ApiClientException(
        int statusCode,
        string message,
        string? errorId = null,
        string? errorCode = null,
        IDictionary<string, string[]>? validationErrors = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorId = errorId;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors;
    }

    public int StatusCode { get; }
    public string? ErrorId { get; }
    public string? ErrorCode { get; }
    public IDictionary<string, string[]>? ValidationErrors { get; }
}
