using System.Collections.ObjectModel;
using System.Globalization;
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

/// <summary>Mirrors features/customer/booking-create/booking-create.ts — pick a date + an
/// available slot (from GET bookings/slots) for a service already chosen on Salon Detail.</summary>
public sealed partial class BookingCreateViewModel : CustomerAreaViewModelBase, IQueryAttributable
{
    private const int DateStripDays = 7;

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

    public ObservableCollection<BookingDateOption> AvailableDates { get; } = new();

    public ObservableCollection<BookingSlotOption> Slots { get; } = new();

    [ObservableProperty]
    private DateOnly selectedDate;

    [ObservableProperty]
    private BookingSlotOption? selectedSlot;

    [ObservableProperty]
    private bool isLoadingSlots;

    [ObservableProperty]
    private bool isSubmitting;

    public bool ShowNoSlotsState => !IsLoadingSlots && Slots.Count == 0;

    public string SubmitButtonText => IsSubmitting
        ? "Booking…"
        : SelectedSlot is not null && Service is not null
            ? $"Confirm booking · {SelectedSlot.DisplayTime}, ₹{Service.Price}  →"
            : "Confirm booking";

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

        var today = DateOnly.FromDateTime(DateTime.Today);
        AvailableDates.Clear();
        for (var i = 0; i < DateStripDays; i++)
        {
            var date = today.AddDays(i);
            AvailableDates.Add(new BookingDateOption
            {
                Date = date,
                DayNumber = date.Day.ToString("00", CultureInfo.InvariantCulture),
                DayName = date.ToString("ddd", CultureInfo.InvariantCulture),
                IsSelected = i == 0,
            });
        }

        SelectedDate = today;

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

        await LoadSlotsAsync();
    }

    [RelayCommand]
    private async Task SelectDateAsync(BookingDateOption dateOption)
    {
        foreach (var date in AvailableDates)
        {
            date.IsSelected = date == dateOption;
        }

        SelectedDate = dateOption.Date;
        SelectedSlot = null;
        await LoadSlotsAsync();
    }

    [RelayCommand]
    private void SelectSlot(BookingSlotOption slotOption)
    {
        if (!slotOption.IsAvailable)
        {
            return;
        }

        foreach (var slot in Slots)
        {
            slot.IsSelected = slot == slotOption;
        }

        SelectedSlot = slotOption;
    }

    private async Task LoadSlotsAsync()
    {
        ErrorMessage = null;
        IsLoadingSlots = true;
        Slots.Clear();
        OnPropertyChanged(nameof(ShowNoSlotsState));

        try
        {
            var slots = await _bookingService.GetAvailableSlotsAsync(_salonId, _serviceId, SelectedDate);
            foreach (var slot in slots)
            {
                // StartTimeUtc arrives as UTC; display it in the device's local time, same as
                // every other slot/booking timestamp shown elsewhere in the app.
                Slots.Add(new BookingSlotOption
                {
                    StartTimeUtc = slot.StartTimeUtc,
                    DisplayTime = slot.StartTimeUtc.ToLocalTime().ToString("h:mm tt", CultureInfo.InvariantCulture),
                    IsAvailable = slot.IsAvailable,
                });
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
            IsLoadingSlots = false;
            OnPropertyChanged(nameof(ShowNoSlotsState));
        }
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitAsync()
    {
        if (SelectedSlot is null)
        {
            return;
        }

        ErrorMessage = null;
        IsSubmitting = true;

        try
        {
            var booking = await _bookingService.CreateAsync(new BookingCreateRequestDto
            {
                SalonId = _salonId,
                ServiceId = _serviceId,
                StaffId = null,
                SlotStartTimeUtc = SelectedSlot.StartTimeUtc,
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

    private bool CanSubmit() => SelectedSlot is not null && !IsSubmitting;

    partial void OnSelectedSlotChanged(BookingSlotOption? value)
    {
        SubmitCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(SubmitButtonText));
    }

    partial void OnIsSubmittingChanged(bool value)
    {
        SubmitCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(SubmitButtonText));
    }

    partial void OnServiceChanged(ServiceResponseDto? value) => OnPropertyChanged(nameof(SubmitButtonText));
}
