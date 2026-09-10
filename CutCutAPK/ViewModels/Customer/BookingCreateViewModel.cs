using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Bookings;
using CutCutAPK.Models.Catalog;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Bookings;
using CutCutAPK.Services.Catalog;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>Mirrors features/customer/booking-create/booking-create.ts — pick a date + time for a
/// service already chosen on Salon Detail.</summary>
public sealed partial class BookingCreateViewModel : CustomerAreaViewModelBase, IQueryAttributable
{
    private readonly ICatalogService _catalogService;
    private readonly IBookingService _bookingService;

    private int _salonId;
    private int _serviceId;

    public BookingCreateViewModel(
        ICatalogService catalogService,
        IBookingService bookingService,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService)
    {
        _catalogService = catalogService;
        _bookingService = bookingService;
    }

    [ObservableProperty]
    private ServiceResponseDto? service;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private TimeSpan selectedTime = new(10, 0, 0);

    [ObservableProperty]
    private bool isSubmitting;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("salonId", out var salonIdValue) && int.TryParse(salonIdValue?.ToString(), out var salonId))
        {
            _salonId = salonId;
        }

        if (query.TryGetValue("serviceId", out var serviceIdValue) && int.TryParse(serviceIdValue?.ToString(), out var serviceId))
        {
            _serviceId = serviceId;
        }
    }

    public async Task LoadAsync()
    {
        ErrorMessage = null;

        try
        {
            Service = await _catalogService.GetByIdAsync(_serviceId);
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        ErrorMessage = null;
        IsSubmitting = true;

        try
        {
            // Combine the picked local date + time, then convert to UTC — mirrors the web app's
            // `new Date(\`${date}T${time}:00\`).toISOString()`, which JS also interprets as a
            // local time before converting.
            var local = DateTime.SpecifyKind(SelectedDate.Date + SelectedTime, DateTimeKind.Local);
            var slotStartTimeUtc = local.ToUniversalTime();

            var booking = await _bookingService.CreateAsync(new BookingCreateRequestDto
            {
                SalonId = _salonId,
                ServiceId = _serviceId,
                StaffId = null,
                SlotStartTimeUtc = slotStartTimeUtc,
            });

            await NavigationService.NavigateToAsync($"{Routes.BookingDetail}?bookingId={booking.BookingId}");
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
