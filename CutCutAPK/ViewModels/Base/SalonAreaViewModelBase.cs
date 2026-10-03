using CutCutAPK.Models.Auth;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Salons;

namespace CutCutAPK.ViewModels.Base;

/// <summary>
/// Shared "which salon" plumbing for the salon area (today's appointments, services, staff,
/// settings) — the SalonContextService gate every one of those screens sits behind. Switching
/// between them is handled by the bottom tab bar in AppShell.xaml.
/// </summary>
public abstract partial class SalonAreaViewModelBase : AuthenticatedViewModelBase
{
    protected readonly ISalonContextService SalonContext;

    protected SalonAreaViewModelBase(IAuthService authService, INavigationService navigationService, ISalonContextService salonContext)
        : base(authService, navigationService)
    {
        SalonContext = salonContext;
    }

    public bool IsOwner => AuthService.CurrentUser?.RoleName == RoleName.SalonOwner;

    /// <summary>Guards every salon-management screen against being reached with no salon selected
    /// (e.g. a stale deep link) — sends the user back to the SalonHome gate, which re-derives the
    /// right state (picker / create prompt / today's appointments) from scratch.</summary>
    protected async Task<bool> EnsureSalonSelectedAsync()
    {
        if (SalonContext.SelectedSalon is not null)
        {
            return true;
        }

        await NavigationService.NavigateToRootAsync(Routes.SalonHome);
        return false;
    }
}
