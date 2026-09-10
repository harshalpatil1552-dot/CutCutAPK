namespace CutCutAPK.Models.Bookings;

/// <summary>Mirrors CutCut.API Models/Dtos/Bookings/OtpResponseDto.cs. Returned once, directly to the
/// customer, at generation/regeneration time — stands in for an SMS/push delivery that isn't wired
/// up yet, same as the web app.</summary>
public sealed class OtpResponseDto
{
    public int BookingId { get; set; }

    public string OtpCode { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}
