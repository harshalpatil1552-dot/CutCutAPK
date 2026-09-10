namespace CutCutAPK.Models.Bookings;

/// <summary>Mirrors CutCut.API Common/Constants/BookingStatusIds.cs (and the web app's
/// booking.model.ts BookingStatusId enum) — Pending -> Confirmed -> CheckedIn -> InProgress ->
/// Completed, with Cancelled reachable from Pending/Confirmed.</summary>
public enum BookingStatusId : byte
{
    Pending = 1,
    Confirmed = 2,
    CheckedIn = 3,
    InProgress = 4,
    Completed = 5,
    Cancelled = 6,
}
