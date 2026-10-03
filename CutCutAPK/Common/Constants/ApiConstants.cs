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
    // A Cloudflare quick tunnel (`cloudflared tunnel --url http://localhost:5186`) pointed at
    // CutCut.API's "http" launch profile — reachable from the emulator, a physical device on any
    // network, or anywhere else, unlike 10.0.2.2 (emulator-only) or a LAN IP (same-network-only).
    // The trade-off: cloudflared's free quick tunnels get a new random hostname every time you
    // restart it, so this literal has to be updated each session — see the run-tunnel.bat helper
    // (if present) or re-run cloudflared and paste the new https://*.trycloudflare.com URL here.
    public static readonly string BaseUrl = "https://cal-sphere-various-heroes.trycloudflare.com/api/v1/";
#else
    // TODO: replace with the deployed API's base URL before shipping a production build —
    // same requirement the web app calls out in environment.prod.ts.
    public static readonly string BaseUrl = "https://cal-sphere-various-heroes.trycloudflare.com/api/v1/";
#endif
}
