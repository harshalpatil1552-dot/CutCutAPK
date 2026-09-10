namespace CutCutAPK.Models.Bookings;

/// <summary>Mirrors CutCut.API Models/Dtos/Bookings/BookingCreateRequestDto.cs — POST /bookings body.</summary>
public sealed class BookingCreateRequestDto
{
    public int SalonId { get; set; }

    public int ServiceId { get; set; }

    /// <summary>Left null to let the salon assign any available staff member — the mobile app never
    /// offers staff selection, matching the web app's booking-create screen.</summary>
    public int? StaffId { get; set; }

    public DateTime SlotStartTimeUtc { get; set; }
}
