namespace CutCutAPK.Models.Bookings;

/// <summary>Mirrors CutCut.API Models/Dtos/Bookings/AvailableSlotDto.cs — one bookable slot
/// returned by GET bookings/slots.</summary>
public sealed class AvailableSlotDto
{
    public DateTime StartTimeUtc { get; set; }

    public DateTime EndTimeUtc { get; set; }

    public bool IsAvailable { get; set; }
}
