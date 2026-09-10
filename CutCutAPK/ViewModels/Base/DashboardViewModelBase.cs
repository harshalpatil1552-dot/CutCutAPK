using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;

namespace CutCutAPK.ViewModels.Base;

/// <summary>
/// Shared "signed-in landing page" behavior for the Customer and Salon areas — both are
/// placeholders standing in for the real feature set (salon search/bookings, staff/appointments)
/// that lives behind CutCut's role-based routing on the web. Sharing this base avoids duplicating
/// the welcome text and logout flow across two otherwise-identical stub pages while still keeping
/// the areas themselves as distinct, independently-growable types.
/// </summary>
public abstract partial class DashboardViewModelBase : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    protected DashboardViewModelBase(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    public string WelcomeMessage => $"Welcome, {_authService.CurrentUser?.FullName}";

    public string RoleLabel => _authService.CurrentUser?.RoleName ?? string.Empty;

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await _navigationService.NavigateToLoginAsync();
    }
}
