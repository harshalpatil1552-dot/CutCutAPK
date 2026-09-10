using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>Landing page for Customer/Admin roles after login. Placeholder for the customer area
/// (salon search, bookings) — see DashboardViewModelBase for the shared welcome/logout behavior.</summary>
public sealed class CustomerDashboardViewModel : DashboardViewModelBase
{
    public CustomerDashboardViewModel(IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
    }
}
