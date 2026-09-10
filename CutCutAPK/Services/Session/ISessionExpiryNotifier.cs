namespace CutCutAPK.Services.Session;

/// <summary>
/// Fan-out point for "the stored token is no longer valid" (expired or revoked server-side).
/// Decouples the layer that detects this (ApiClient, on a 401) from the layers that react to it
/// (AuthService clearing state, Shell navigating back to the login screen) — the same event the
/// web app's errorInterceptor handles inline, split here because navigation is Shell's
/// responsibility, not the HTTP layer's.
/// </summary>
public interface ISessionExpiryNotifier
{
    event Action? SessionExpired;

    void NotifySessionExpired();
}

/// <inheritdoc cref="ISessionExpiryNotifier" />
public sealed class SessionExpiryNotifier : ISessionExpiryNotifier
{
    public event Action? SessionExpired;

    public void NotifySessionExpired() => SessionExpired?.Invoke();
}
