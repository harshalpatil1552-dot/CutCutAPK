using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Catalog;
using CutCutAPK.Models.Reviews;
using CutCutAPK.Models.Salons;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Catalog;
using CutCutAPK.Services.Reviews;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>Mirrors features/customer/salon-detail/salon-detail.ts — a salon's info, its bookable
/// services, and its reviews.</summary>
public sealed partial class SalonDetailViewModel : CustomerAreaViewModelBase, IQueryAttributable
{
    private readonly ISalonService _salonService;
    private readonly ICatalogService _catalogService;
    private readonly IReviewService _reviewService;

    private int _salonId;

    public SalonDetailViewModel(
        ISalonService salonService,
        ICatalogService catalogService,
        IReviewService reviewService,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService)
    {
        _salonService = salonService;
        _catalogService = catalogService;
        _reviewService = reviewService;
    }

    [ObservableProperty]
    private SalonResponseDto? salon;

    public ObservableCollection<ServiceResponseDto> Services { get; } = new();

    public ObservableCollection<ReviewResponseDto> Reviews { get; } = new();

    [ObservableProperty]
    private bool isLoading = true;

    public bool ShowEmptyServices => !IsLoading && Services.Count == 0;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("salonId", out var value) && int.TryParse(value?.ToString(), out var salonId))
        {
            _salonId = salonId;
        }
    }

    public async Task LoadAsync()
    {
        ErrorMessage = null;
        IsLoading = true;

        try
        {
            Salon = await _salonService.GetByIdAsync(_salonId);

            var services = await _catalogService.GetBySalonAsync(_salonId);
            Services.Clear();
            foreach (var service in services)
            {
                Services.Add(service);
            }

            try
            {
                var reviews = await _reviewService.GetBySalonAsync(_salonId);
                Reviews.Clear();
                foreach (var review in reviews)
                {
                    Reviews.Add(review);
                }
            }
            catch (Exception)
            {
                // Reviews are supplementary — the web app's salon-detail.ts doesn't even set
                // errorMessage on this call's failure, so a reviews-load failure shouldn't block
                // the rest of the page either.
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
            OnPropertyChanged(nameof(ShowEmptyServices));
        }
    }

    [RelayCommand]
    private Task BookAsync(ServiceResponseDto service) =>
        NavigationService.NavigateToAsync($"{Routes.BookingCreate}?salonId={_salonId}&serviceId={service.ServiceId}");

    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(ShowEmptyServices));
}
