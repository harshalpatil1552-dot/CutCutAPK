using CutCutAPK.Models.Auth;

namespace CutCutAPK.Services.Auth;

/// <summary>
/// Talks to /auth/login and holds the signed-in user for the lifetime of the app, so any
/// ViewModel or guard can check auth state without depending on HttpClient itself. Mirrors the
/// web app's AuthService.
/// </summary>
public interface IAuthService
{
    UserProfileDto? CurrentUser { get; }

    bool IsAuthenticated { get; }

    /// <summary>Raised after sign-in and after sign-out (including an implicit sign-out from
    /// session expiry) so subscribers can react without polling.</summary>
    event Action? AuthStateChanged;

    /// <summary>Hydrates <see cref="CurrentUser"/> from persisted storage. Must be awaited once at
    /// app startup — there's no synchronous storage read available on mobile the way
    /// localStorage gives the web app.</summary>
    Task InitializeAsync();

    Task<UserProfileDto> LoginAsync(string phoneOrEmail, string password, CancellationToken cancellationToken = default);

    Task<UserProfileDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    Task LogoutAsync();
}
