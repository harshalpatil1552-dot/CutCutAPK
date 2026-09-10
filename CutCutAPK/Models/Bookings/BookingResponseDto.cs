namespace CutCutAPK.Models.Bookings;

/// <summary>Mirrors CutCut.API Models/Dtos/Bookings/BookingResponseDto.cs.</summary>
public sealed class BookingResponseDto
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public int SalonId { get; set; }

    public string SalonName { get; set; } = string.Empty;

    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = string.Empty;

    public int? StaffId { get; set; }

    public BookingStatusId BookingStatusId { get; set; }

    public string BookingStatusName { get; set; } = string.Empty;

    public DateTime SlotStartTimeUtc { get; set; }

    public DateTime SlotEndTimeUtc { get; set; }

    public decimal Amount { get; set; }

    public DateTime? CheckedInAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? CancelledAt { get; set; }
}
