using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Auth;
using CutCutAPK.Models.Salons;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Mirrors features/salon/salon-shell/salon-shell.ts — gates the whole salon area behind
/// "which salon am I managing". Loads /salons/mine; if exactly one salon comes back
/// (ISalonContextService auto-selects it) this screen immediately hands off to Today's
/// Appointments, same as the web app's shell rendering its router-outlet once a salon is
/// selected. Otherwise it renders whichever of the three states below applies.</summary>
public sealed partial class SalonHomeViewModel : AuthenticatedViewModelBase
{
    private readonly ISalonContextService _salonContext;

    public SalonHomeViewModel(ISalonContextService salonContext, IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        _salonContext = salonContext;
    }

    public bool IsOwner => AuthService.CurrentUser?.RoleName == RoleName.SalonOwner;

    [ObservableProperty]
    private bool isLoaded;

    public ObservableCollection<SalonResponseDto> MySalons { get; } = new();

    /// <summary>More than one salon managed by this account — pick which to work on.</summary>
    public bool ShowPicker => IsLoaded && MySalons.Count > 0;

    /// <summary>No salon yet, and this account can create one.</summary>
    public bool ShowCreatePrompt => IsLoaded && MySalons.Count == 0 && IsOwner;

    /// <summary>No salon yet, and this account is staff — only an owner can add them to one.</summary>
    public bool ShowStaffPrompt => IsLoaded && MySalons.Count == 0 && !IsOwner;

    public async Task LoadAsync()
    {
        ErrorMessage = null;

        try
        {
            var salons = await _salonContext.LoadMineAsync();

            MySalons.Clear();
            foreach (var salon in salons)
            {
                MySalons.Add(salon);
            }

            if (_salonContext.SelectedSalon is not null)
            {
                await NavigationService.NavigateToRootAsync(Routes.TodayAppointments);
                return;
            }
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsLoaded = true;
            RaisePresentationChanged();
        }
    }

    [RelayCommand]
    private async Task SelectSalonAsync(SalonResponseDto salon)
    {
        _salonContext.SelectSalon(salon.SalonId);
        await NavigationService.NavigateToRootAsync(Routes.TodayAppointments);
    }

    [RelayCommand]
    private Task GoToCreateSalonAsync() => NavigationService.NavigateToAsync(Routes.SalonCreate);

    private void RaisePresentationChanged()
    {
        OnPropertyChanged(nameof(ShowPicker));
        OnPropertyChanged(nameof(ShowCreatePrompt));
        OnPropertyChanged(nameof(ShowStaffPrompt));
    }
}
