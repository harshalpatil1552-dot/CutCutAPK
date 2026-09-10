using CommunityToolkit.Mvvm.ComponentModel;
using CutCutAPK.Models.Bookings;

namespace CutCutAPK.ViewModels.Salon;

/// <summary>Wraps one booking on Today's Appointments with its own OTP-entry text — mirrors the web
/// app's `otpInputs` signal (a Record&lt;bookingId, string&gt;), but as a per-row observable object
/// instead, since MAUI's CollectionView has no direct binding path to a dictionary keyed by an
/// item's own id. <see cref="Booking"/> itself is replaced wholesale (rather than mutated) after
/// each status-changing action, same as the web app's `bookings.update(...)` replacing the array
/// entry.</summary>
public sealed partial class TodayAppointmentRowViewModel : ObservableObject
{
    [ObservableProperty]
    private BookingResponseDto booking;

    [ObservableProperty]
    private string otpInput = string.Empty;

    public TodayAppointmentRowViewModel(BookingResponseDto booking)
    {
        this.booking = booking;
    }
}
