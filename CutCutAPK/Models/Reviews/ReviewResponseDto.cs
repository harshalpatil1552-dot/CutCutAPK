namespace CutCutAPK.Models.Reviews;

/// <summary>Mirrors CutCut.API Models/Dtos/Reviews/ReviewResponseDto.cs.</summary>
public sealed class ReviewResponseDto
{
    public int ReviewId { get; set; }

    public int BookingId { get; set; }

    public int SalonId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public byte Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
