namespace CutCutAPK.Common.Constants;

/// <summary>
/// API connection settings. Mirrors the split the Angular app gets for free from
/// src/environments/environment.ts vs environment.prod.ts — Debug points at the local CutCut.API
/// instance, Release must be updated to the deployed API's HTTPS URL before shipping.
/// </summary>
public static class ApiConstants
{
    /// <summary>Name of the named HttpClient registered in MauiProgram — keeps DI registration and
    /// consumption in sync without repeating a magic string everywhere.</summary>
    public const string HttpClientName = "CutCutApi";

    public const int TimeoutSeconds = 30;

    // Trailing slash is required, not cosmetic: HttpClient resolves a relative request URI
    // ("auth/login") against BaseAddress using standard URI-merge rules, which drop everything
    // after the last '/' in the base path first. Without the trailing slash here, "api/v1" loses
    // "v1" the moment any relative route is combined with it (e.g. "https://host/api/v1" +
    // "auth/login" resolves to ".../api/auth/login", not ".../api/v1/auth/login").
#if DEBUG
    // The Android emulator can't resolve the host machine's "localhost" — 10.0.2.2 is the
    // emulator's documented alias for it. Testing on a physical device instead requires swapping
    // this to the host machine's real LAN IP. Port 7258 + scheme match CutCut.API's "https"
    // launch profile (Properties/launchSettings.json) — its self-signed dev certificate is
    // trusted via DevCertHandler.Create() below, DEBUG-only.
    public static readonly string BaseUrl =
        DeviceInfo.Platform == DevicePlatform.Android
            ? "https://10.0.2.2:7258/api/v1/"
            : "https://localhost:7258/api/v1/";
#else
    // TODO: replace with the deployed API's base URL before shipping a production build —
    // same requirement the web app calls out in environment.prod.ts.
    public static readonly string BaseUrl = "https://api.cutcut.example.com/api/v1/";
#endif
}
