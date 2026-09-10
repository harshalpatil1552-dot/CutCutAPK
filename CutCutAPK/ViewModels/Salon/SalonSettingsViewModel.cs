using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Salons;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Mirrors features/salon/salon-settings/salon-settings.ts — edit the selected salon's
/// details, including the IsActive switch that hides/shows it from customer search.</summary>
public sealed partial class SalonSettingsViewModel : SalonAreaViewModelBase
{
    private readonly ISalonService _salonService;

    public SalonSettingsViewModel(
        ISalonService salonService,
        ISalonContextService salonContext,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService, salonContext)
    {
        _salonService = salonService;
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string addressLine = string.Empty;

    [ObservableProperty]
    private string city = string.Empty;

    /// <summary>MAUI's TimePicker always has a value (unlike the web app's optionally-empty
    /// &lt;input type="time"&gt;), so an unset opening/closing time falls back to a sensible
    /// default rather than being left blank.</summary>
    [ObservableProperty]
    private TimeSpan openingTime = new(9, 0, 0);

    [ObservableProperty]
    private TimeSpan closingTime = new(20, 0, 0);

    [ObservableProperty]
    private bool isActive = true;

    [ObservableProperty]
    private bool isSubmitting;

    [ObservableProperty]
    private string? infoMessage;

    public async Task LoadAsync()
    {
        if (!await EnsureSalonSelectedAsync())
        {
            return;
        }

        var salon = SalonContext.SelectedSalon!;

        Name = salon.Name;
        AddressLine = salon.AddressLine ?? string.Empty;
        City = salon.City ?? string.Empty;
        OpeningTime = ParseTimeOrDefault(salon.OpeningTime, OpeningTime);
        ClosingTime = ParseTimeOrDefault(salon.ClosingTime, ClosingTime);
        IsActive = salon.IsActive;
    }

    private static TimeSpan ParseTimeOrDefault(string? wireValue, TimeSpan fallback) =>
        !string.IsNullOrWhiteSpace(wireValue) && TimeSpan.TryParse(wireValue, out var parsed) ? parsed : fallback;

    [RelayCommand]
    private async Task SubmitAsync()
    {
        var salon = SalonContext.SelectedSalon;
        if (salon is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length < 2)
        {
            ErrorMessage = "Enter a salon name.";
            return;
        }

        ErrorMessage = null;
        InfoMessage = null;
        IsSubmitting = true;

        try
        {
            var updated = await _salonService.UpdateAsync(salon.SalonId, new SalonUpdateRequestDto
            {
                Name = Name.Trim(),
                AddressLine = string.IsNullOrWhiteSpace(AddressLine) ? null : AddressLine.Trim(),
                City = string.IsNullOrWhiteSpace(City) ? null : City.Trim(),
                OpeningTime = OpeningTime.ToString(@"hh\:mm\:ss"),
                ClosingTime = ClosingTime.ToString(@"hh\:mm\:ss"),
                IsActive = IsActive,
            });

            SalonContext.UpsertSalon(updated);
            InfoMessage = "Salon details updated.";
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
            IsSubmitting = false;
        }
    }
}
