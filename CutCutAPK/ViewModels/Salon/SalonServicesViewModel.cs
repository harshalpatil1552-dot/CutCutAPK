using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Catalog;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Catalog;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Mirrors features/salon/salon-services/salon-services.ts — the selected salon's
/// bookable services, plus a small inline form to add another.</summary>
public sealed partial class SalonServicesViewModel : SalonAreaViewModelBase
{
    private readonly ICatalogService _catalogService;

    public SalonServicesViewModel(
        ICatalogService catalogService,
        ISalonContextService salonContext,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService, salonContext)
    {
        _catalogService = catalogService;
    }

    public ObservableCollection<ServiceResponseDto> Services { get; } = new();

    [ObservableProperty]
    private bool isLoading = true;

    // ---- Add-service form ----

    [ObservableProperty]
    private string newServiceName = string.Empty;

    [ObservableProperty]
    private int newServiceDurationMinutes = 30;

    [ObservableProperty]
    private decimal newServicePrice;

    [ObservableProperty]
    private bool isSubmitting;

    // ---- Per-row busy state ----

    [ObservableProperty]
    private int? busyServiceId;

    public async Task LoadAsync()
    {
        if (!await EnsureSalonSelectedAsync())
        {
            return;
        }

        ErrorMessage = null;
        IsLoading = true;

        try
        {
            var services = await _catalogService.GetBySalonAsync(SalonContext.SelectedSalon!.SalonId);
            Services.Clear();
            foreach (var service in services)
            {
                Services.Add(service);
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
        }
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (SalonContext.SelectedSalon is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(NewServiceName) || NewServiceName.Trim().Length < 2)
        {
            ErrorMessage = "Enter a name.";
            return;
        }

        if (NewServiceDurationMinutes is < 5 or > 480)
        {
            ErrorMessage = "Duration must be between 5 and 480 minutes.";
            return;
        }

        if (NewServicePrice < 0)
        {
            ErrorMessage = "Price cannot be negative.";
            return;
        }

        ErrorMessage = null;
        IsSubmitting = true;

        try
        {
            var created = await _catalogService.CreateAsync(SalonContext.SelectedSalon.SalonId, new ServiceCreateRequestDto
            {
                Name = NewServiceName.Trim(),
                DurationMinutes = (short)NewServiceDurationMinutes,
                Price = NewServicePrice,
            });

            Services.Add(created);
            NewServiceName = string.Empty;
            NewServiceDurationMinutes = 30;
            NewServicePrice = 0;
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

    [RelayCommand]
    private async Task ToggleActiveAsync(ServiceResponseDto service)
    {
        BusyServiceId = service.ServiceId;

        try
        {
            var updated = await _catalogService.UpdateAsync(service.ServiceId, new ServiceUpdateRequestDto
            {
                Name = service.Name,
                DurationMinutes = service.DurationMinutes,
                Price = service.Price,
                IsActive = !service.IsActive,
            });

            var index = Services.IndexOf(service);
            if (index >= 0)
            {
                Services[index] = updated;
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
            BusyServiceId = null;
        }
    }
}
