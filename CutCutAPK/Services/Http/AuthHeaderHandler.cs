using System.Net.Http.Headers;
using CutCutAPK.Services.Session;

namespace CutCutAPK.Services.Http;

/// <summary>
/// Attaches the stored JWT (if any) to every outgoing request as a Bearer token — the
/// DelegatingHandler equivalent of the web app's authInterceptor.
/// </summary>
public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly ISessionStore _sessionStore;

    public AuthHeaderHandler(ISessionStore sessionStore)
    {
        _sessionStore = sessionStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _sessionStore.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
