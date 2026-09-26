using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Bookings;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Bookings;
using CutCutAPK.Services.Salons;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Mirrors features/salon/today-appointments/today-appointments.ts — the selected salon's
/// bookings for today, with check-in/start/complete/cancel actions gated by each booking's current
/// status.</summary>
public sealed partial class TodayAppointmentsViewModel : SalonAreaViewModelBase
{
    private readonly IBookingService _bookingService;

    public TodayAppointmentsViewModel(
        IBookingService bookingService,
        ISalonContextService salonContext,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService, salonContext)
    {
        _bookingService = bookingService;
    }

    public ObservableCollection<TodayAppointmentRowViewModel> Rows { get; } = new();

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private int? busyBookingId;

    public bool ShowEmptyState => !IsLoading && Rows.Count == 0;

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
            var bookings = await _bookingService.GetSalonTodayAsync(SalonContext.SelectedSalon!.SalonId);
            Rows.Clear();
            foreach (var booking in bookings)
            {
                Rows.Add(new TodayAppointmentRowViewModel(booking));
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
            OnPropertyChanged(nameof(ShowEmptyState));
        }
    }

    [RelayCommand]
    private Task RefreshAsync() => LoadAsync();

    [RelayCommand]
    private async Task CheckInAsync(TodayAppointmentRowViewModel row)
    {
        ErrorMessage = null;
        BusyBookingId = row.Booking.BookingId;

        try
        {
            row.Booking = await _bookingService.CheckInAsync(row.Booking.BookingId, row.OtpInput ?? string.Empty);
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
            BusyBookingId = null;
        }
    }

    [RelayCommand]
    private Task StartAsync(TodayAppointmentRowViewModel row) => RunActionAsync(row, () => _bookingService.StartAsync(row.Booking.BookingId));

    [RelayCommand]
    private Task CompleteAsync(TodayAppointmentRowViewModel row) => RunActionAsync(row, () => _bookingService.CompleteAsync(row.Booking.BookingId));

    [RelayCommand]
    private Task CancelAsync(TodayAppointmentRowViewModel row) => RunActionAsync(row, () => _bookingService.CancelAsync(row.Booking.BookingId));

    private async Task RunActionAsync(TodayAppointmentRowViewModel row, Func<Task<BookingResponseDto>> action)
    {
        ErrorMessage = null;
        BusyBookingId = row.Booking.BookingId;

        try
        {
            row.Booking = await action();
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
            BusyBookingId = null;
        }
    }

    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(ShowEmptyState));
}
