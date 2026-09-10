namespace CutCutAPK.Models.Bookings;

/// <summary>Mirrors CutCut.API Models/Dtos/Bookings/BookingCheckInRequestDto.cs — POST /bookings/{id}/checkin body.</summary>
public sealed class BookingCheckInRequestDto
{
    public string OtpCode { get; set; } = string.Empty;
}
