using CutCutAPK.Models.Auth;

namespace CutCutAPK.Services.Session;

/// <summary>
/// Persists the signed-in session. Functionally equivalent to the web app's TokenStorageService,
/// but backed by platform SecureStorage (Android Keystore / iOS Keychain) instead of localStorage —
/// a mobile app has no browser sandbox to lean on, so the access token needs at-rest encryption
/// explicitly.
/// </summary>
public interface ISessionStore
{
    Task<string?> GetAccessTokenAsync();

    Task<UserProfileDto?> GetCurrentUserAsync();

    Task SetSessionAsync(string accessToken, UserProfileDto user);

    Task ClearSessionAsync();
}
