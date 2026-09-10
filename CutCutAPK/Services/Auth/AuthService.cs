using CutCutAPK.Models.Auth;
using CutCutAPK.Services.Api;
using CutCutAPK.Services.Session;

namespace CutCutAPK.Services.Auth;

/// <inheritdoc cref="IAuthService" />
public sealed class AuthService : IAuthService
{
    private const string LoginRoute = "auth/login";
    private const string RegisterRoute = "auth/register";

    private readonly IApiClient _apiClient;
    private readonly ISessionStore _sessionStore;

    public UserProfileDto? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser is not null;

    public event Action? AuthStateChanged;

    public AuthService(IApiClient apiClient, ISessionStore sessionStore, ISessionExpiryNotifier sessionExpiryNotifier)
    {
        _apiClient = apiClient;
        _sessionStore = sessionStore;

        // A 401 on any authenticated call means the stored token is no longer valid — drop the
        // stale session so IsAuthenticated reflects reality even before the user takes any action.
        sessionExpiryNotifier.SessionExpired += () => _ = LogoutAsync();
    }

    public async Task InitializeAsync()
    {
        CurrentUser = await _sessionStore.GetCurrentUserAsync();
    }

    public async Task<UserProfileDto> LoginAsync(string phoneOrEmail, string password, CancellationToken cancellationToken = default)
    {
        var request = new LoginRequestDto { PhoneOrEmail = phoneOrEmail, Password = password };
        var response = await _apiClient.PostAsync<AuthResponseDto>(LoginRoute, request, cancellationToken);

        await _sessionStore.SetSessionAsync(response.AccessToken, response.User);
        CurrentUser = response.User;
        AuthStateChanged?.Invoke();

        return response.User;
    }

    public async Task<UserProfileDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PostAsync<AuthResponseDto>(RegisterRoute, request, cancellationToken);

        await _sessionStore.SetSessionAsync(response.AccessToken, response.User);
        CurrentUser = response.User;
        AuthStateChanged?.Invoke();

        return response.User;
    }

    public async Task LogoutAsync()
    {
        await _sessionStore.ClearSessionAsync();
        CurrentUser = null;
        AuthStateChanged?.Invoke();
    }
}
