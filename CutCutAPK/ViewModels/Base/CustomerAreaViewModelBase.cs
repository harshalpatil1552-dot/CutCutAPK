using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;

namespace CutCutAPK.ViewModels.Base;

/// <summary>
/// Shared navigation for the customer area (salon search, salon detail, booking create/detail,
/// my bookings). "Find a salon" and "My bookings" are bottom tabs (see AppShell.xaml), so both
/// jump to the tab root; GoBack is for the pushed detail pages, which hide the Shell nav bar.
/// </summary>
public abstract partial class CustomerAreaViewModelBase : AuthenticatedViewModelBase
{
    protected CustomerAreaViewModelBase(IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
    }

    [RelayCommand]
    private Task GoToSalonSearchAsync() => NavigationService.NavigateToRootAsync(Routes.SalonSearch);

    [RelayCommand]
    private Task GoToMyBookingsAsync() => NavigationService.NavigateToRootAsync(Routes.MyBookings);

    [RelayCommand]
    private Task GoBackAsync() => NavigationService.GoBackAsync();
}
