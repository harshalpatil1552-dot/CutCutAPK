using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Bookings;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Bookings;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>Mirrors features/customer/my-bookings/my-bookings.ts — every booking the signed-in
/// customer has made, newest state first as the API returns it.</summary>
public sealed partial class MyBookingsViewModel : CustomerAreaViewModelBase
{
    private readonly IBookingService _bookingService;

    public MyBookingsViewModel(IBookingService bookingService, IAuthService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        _bookingService = bookingService;
    }

    public ObservableCollection<BookingResponseDto> Bookings { get; } = new();

    [ObservableProperty]
    private bool isLoading = true;

    public bool ShowEmptyState => !IsLoading && Bookings.Count == 0;

    public async Task LoadAsync()
    {
        ErrorMessage = null;
        IsLoading = true;

        try
        {
            var bookings = await _bookingService.GetMineAsync();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
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
    private Task OpenBookingAsync(BookingResponseDto booking) =>
        NavigationService.NavigateToAsync($"{Routes.BookingDetail}?bookingId={booking.BookingId}");

    partial void OnIsLoadingChanged(bool value) => OnPropertyChanged(nameof(ShowEmptyState));
}
