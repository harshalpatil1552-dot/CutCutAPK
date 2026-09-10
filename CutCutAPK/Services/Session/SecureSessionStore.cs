using System.Text.Json;
using CutCutAPK.Common.Constants;
using CutCutAPK.Models.Auth;

namespace CutCutAPK.Services.Session;

/// <inheritdoc cref="ISessionStore" />
public sealed class SecureSessionStore : ISessionStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            return await SecureStorage.Default.GetAsync(StorageKeys.AccessToken);
        }
        catch (Exception)
        {
            // SecureStorage can throw if the platform keystore entry was invalidated (e.g. the
            // device's lock-screen credentials changed) — treat that the same as "no session".
            return null;
        }
    }

    public async Task<UserProfileDto?> GetCurrentUserAsync()
    {
        string? raw;
        try
        {
            raw = await SecureStorage.Default.GetAsync(StorageKeys.CurrentUser);
        }
        catch (Exception)
        {
            return null;
        }

        if (string.IsNullOrEmpty(raw))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<UserProfileDto>(raw, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public async Task SetSessionAsync(string accessToken, UserProfileDto user)
    {
        await SecureStorage.Default.SetAsync(StorageKeys.AccessToken, accessToken);
        await SecureStorage.Default.SetAsync(StorageKeys.CurrentUser, JsonSerializer.Serialize(user, JsonOptions));
    }

    public Task ClearSessionAsync()
    {
        SecureStorage.Default.Remove(StorageKeys.AccessToken);
        SecureStorage.Default.Remove(StorageKeys.CurrentUser);
        return Task.CompletedTask;
    }
}
