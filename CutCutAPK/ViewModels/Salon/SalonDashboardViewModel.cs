using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Landing page for SalonStaff/SalonOwner roles after login. Placeholder for the salon
/// area (staff, today's appointments) — see DashboardViewModelBase for the shared welcome/logout
/// behavior.</summary>
public sealed class SalonDashboardViewModel : DashboardViewModelBase
{
    public SalonDashboardViewModel(IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
    }
}
