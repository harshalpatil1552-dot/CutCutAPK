using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;

namespace CutCutAPK.ViewModels.Base;

/// <summary>
/// Shared "Log out" behavior for every signed-in screen (salon search, bookings, salon
/// management, today's appointments) — the header row on each of those pages binds its Log out
/// button to this same LogoutCommand, the same way the web app's AppHeaderComponent is reused by
/// both CustomerShell and SalonShell instead of each nav area re-implementing logout.
/// </summary>
public abstract partial class AuthenticatedViewModelBase : ViewModelBase
{
    protected readonly IAuthService AuthService;
    protected readonly INavigationService NavigationService;

    protected AuthenticatedViewModelBase(IAuthService authService, INavigationService navigationService)
    {
        AuthService = authService;
        NavigationService = navigationService;
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await AuthService.LogoutAsync();
        await NavigationService.NavigateToLoginAsync();
    }
}
