namespace CutCutAPK.Common.Exceptions;

/// <summary>
/// Thrown by <see cref="Services.Api.ApiClient"/> for any failed request — mirrors the web app's
/// errorInterceptor, which unwraps the ApiResponse envelope's `message` into a plain Error so every
/// caller can just read `.Message` instead of re-parsing the response shape itself each time.
/// </summary>
public sealed class ApiException : Exception
{
    public int? StatusCode { get; }

    public ApiException(string message, int? statusCode = null)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
