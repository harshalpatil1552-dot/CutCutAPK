using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Salons;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>Mirrors features/customer/salon-search/salon-search.ts — search by city, or "near me"
/// via device location.</summary>
public sealed partial class SalonSearchViewModel : CustomerAreaViewModelBase
{
    private readonly ISalonService _salonService;

    public SalonSearchViewModel(ISalonService salonService, IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        _salonService = salonService;
    }

    [ObservableProperty]
    private string city = string.Empty;

    public ObservableCollection<SalonResponseDto> Salons { get; } = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool usingLocation;

    [ObservableProperty]
    private bool hasSearched;

    /// <summary>True once loading has finished and nothing came back — drives the empty state,
    /// same condition as the web app's `salons().length === 0 &amp;&amp; hasSearched()`.</summary>
    public bool ShowEmptyState => !IsLoading && HasSearched && Salons.Count == 0;

    public async Task LoadAsync()
    {
        if (!HasSearched)
        {
            await RunSearchAsync(null, null, null);
        }
    }

    [RelayCommand]
    private async Task SearchByCityAsync()
    {
        UsingLocation = false;
        await RunSearchAsync(null, null, null);
    }

    [RelayCommand]
    private async Task SearchNearMeAsync()
    {
        ErrorMessage = null;
        IsLoading = true;

        try
        {
            var permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                IsLoading = false;
                ErrorMessage = "Location is not available in this browser.";
                return;
            }

            var location = await Geolocation.Default.GetLocationAsync(
                new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

            if (location is null)
            {
                IsLoading = false;
                ErrorMessage = "Could not get your location. Search by city instead.";
                return;
            }

            UsingLocation = true;
            await RunSearchAsync(location.Latitude, location.Longitude, 25);
        }
        catch (Exception)
        {
            IsLoading = false;
            ErrorMessage = "Could not get your location. Search by city instead.";
        }
    }

    [RelayCommand]
    private Task GoToSalonAsync(SalonResponseDto salon) =>
        NavigationService.NavigateToAsync($"{Routes.SalonDetail}?salonId={salon.SalonId}");

    private async Task RunSearchAsync(double? latitude, double? longitude, double? radiusKm)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var trimmedCity = City.Trim();
            var results = await _salonService.SearchAsync(new SalonSearchParams
            {
                City = trimmedCity.Length > 0 ? trimmedCity : null,
                Latitude = latitude,
                Longitude = longitude,
                RadiusKm = radiusKm,
            });

            Salons.Clear();
            foreach (var salon in results)
            {
                Salons.Add(salon);
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
            IsLoading = false;
            HasSearched = true;
            OnPropertyChanged(nameof(ShowEmptyState));
        }
    }

    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(ShowEmptyState));

    partial void OnHasSearchedChanged(bool value) => OnPropertyChanged(nameof(ShowEmptyState));
}
