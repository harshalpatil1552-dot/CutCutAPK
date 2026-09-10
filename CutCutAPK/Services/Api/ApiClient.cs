using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CutCutAPK.Common.Constants;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Common;
using CutCutAPK.Services.Session;

namespace CutCutAPK.Services.Api;

/// <inheritdoc cref="IApiClient" />
public sealed class ApiClient : IApiClient
{
    private const string GenericErrorMessage = "Something went wrong. Please try again.";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;
    private readonly ISessionStore _sessionStore;
    private readonly ISessionExpiryNotifier _sessionExpiryNotifier;

    public ApiClient(IHttpClientFactory httpClientFactory, ISessionStore sessionStore, ISessionExpiryNotifier sessionExpiryNotifier)
    {
        _httpClient = httpClientFactory.CreateClient(ApiConstants.HttpClientName);
        _sessionStore = sessionStore;
        _sessionExpiryNotifier = sessionExpiryNotifier;
    }

    public async Task<T> GetAsync<T>(string route, CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Get, route, null, cancellationToken);
        return await ReadEnvelopeAsync<T>(response, cancellationToken);
    }

    public async Task<T> PostAsync<T>(string route, object? body, CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Post, route, body, cancellationToken);
        return await ReadEnvelopeAsync<T>(response, cancellationToken);
    }

    public async Task<T> PutAsync<T>(string route, object body, CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(HttpMethod.Put, route, body, cancellationToken);
        return await ReadEnvelopeAsync<T>(response, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string route, object? body, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(method, route);

            if (body is not null)
            {
                request.Content = JsonContent.Create(body, options: JsonOptions);
            }

            return await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            // The caller didn't cancel — the HttpClient timeout did.
            throw new ApiException("The request timed out. Please check your connection and try again.");
        }
        catch (HttpRequestException)
        {
            throw new ApiException("Unable to reach the server. Please check your internet connection.");
        }
    }

    private async Task<T> ReadEnvelopeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var envelope = await TryReadEnvelopeAsync<T>(response, cancellationToken);

        // A 401 while a token is already stored means that token expired/was revoked server-side
        // rather than "wrong password on the login form" — in that case there's no stored token
        // yet, so this branch doesn't fire and the caller gets the plain error message below.
        // Mirrors the web app's errorInterceptor session-expiry check.
        if (response.StatusCode == HttpStatusCode.Unauthorized && await _sessionStore.GetAccessTokenAsync() is not null)
        {
            await _sessionStore.ClearSessionAsync();
            _sessionExpiryNotifier.NotifySessionExpired();
            throw new ApiException("Your session has expired. Please sign in again.", (int)HttpStatusCode.Unauthorized);
        }

        if (!response.IsSuccessStatusCode || envelope is null || !envelope.Success || envelope.Data is null)
        {
            var message = envelope?.Message is { Length: > 0 } apiMessage ? apiMessage : GenericErrorMessage;
            throw new ApiException(message, (int)response.StatusCode);
        }

        return envelope.Data;
    }

    private static async Task<ApiResponse<T>?> TryReadEnvelopeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions, cancellationToken);
        }
        catch (JsonException)
        {
            // Non-JSON body (e.g. a proxy's HTML error page) — treated as "no envelope" below.
            return null;
        }
    }
}
