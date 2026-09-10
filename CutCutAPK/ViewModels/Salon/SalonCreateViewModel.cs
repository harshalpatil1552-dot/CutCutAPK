using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Salons;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Mirrors features/salon/salon-create/salon-create.ts — a SalonOwner setting up their
/// first (or another) salon.</summary>
public sealed partial class SalonCreateViewModel : AuthenticatedViewModelBase
{
    private readonly ISalonService _salonService;
    private readonly ISalonContextService _salonContext;

    public SalonCreateViewModel(ISalonService salonService, ISalonContextService salonContext, IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        _salonService = salonService;
        _salonContext = salonContext;
    }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string addressLine = string.Empty;

    [ObservableProperty]
    private string city = string.Empty;

    [ObservableProperty]
    private TimeSpan openingTime = new(9, 0, 0);

    [ObservableProperty]
    private TimeSpan closingTime = new(20, 0, 0);

    [ObservableProperty]
    private bool isSubmitting;

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || Name.Trim().Length < 2)
        {
            ErrorMessage = "Enter a salon name (at least 2 characters).";
            return;
        }

        ErrorMessage = null;
        IsSubmitting = true;

        try
        {
            var salon = await _salonService.CreateAsync(new SalonCreateRequestDto
            {
                Name = Name.Trim(),
                AddressLine = string.IsNullOrWhiteSpace(AddressLine) ? null : AddressLine.Trim(),
                City = string.IsNullOrWhiteSpace(City) ? null : City.Trim(),
                OpeningTime = OpeningTime.ToString(@"hh\:mm\:ss"),
                ClosingTime = ClosingTime.ToString(@"hh\:mm\:ss"),
            });

            _salonContext.UpsertSalon(salon);
            await NavigationService.NavigateToRootAsync(Routes.TodayAppointments);
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
