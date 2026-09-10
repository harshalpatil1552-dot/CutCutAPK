namespace CutCutAPK.Services.Api;

/// <summary>
/// Thin JSON HTTP client that unwraps the API's ApiResponse&lt;T&gt; envelope and turns any
/// failure (validation error, server error, timeout, no connectivity) into an ApiException with a
/// user-presentable message — the same job the web app's HttpClient + error interceptor pair does.
/// </summary>
public interface IApiClient
{
    Task<T> GetAsync<T>(string route, CancellationToken cancellationToken = default);

    Task<T> PostAsync<T>(string route, object? body, CancellationToken cancellationToken = default);

    Task<T> PutAsync<T>(string route, object body, CancellationToken cancellationToken = default);
}
