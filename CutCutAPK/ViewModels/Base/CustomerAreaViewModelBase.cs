using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;

namespace CutCutAPK.ViewModels.Base;

/// <summary>
/// Shared header-nav behavior for the customer area (salon search, salon detail, booking
/// create/detail, my bookings) — mirrors the two links the web app's CustomerShell puts in
/// app-header's nav: "Find a salon" and "My bookings".
/// </summary>
public abstract partial class CustomerAreaViewModelBase : AuthenticatedViewModelBase
{
    protected CustomerAreaViewModelBase(IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
    }

    [RelayCommand]
    private Task GoToSalonSearchAsync() => NavigationService.NavigateToAsync(Routes.SalonSearch);

    [RelayCommand]
    private Task GoToMyBookingsAsync() => NavigationService.NavigateToAsync(Routes.MyBookings);
}
