namespace CutCutAPK.Models.Reviews;

/// <summary>Mirrors CutCut.API Models/Dtos/Reviews/ReviewCreateRequestDto.cs — POST /reviews body.</summary>
public sealed class ReviewCreateRequestDto
{
    public int BookingId { get; set; }

    public byte Rating { get; set; }

    public string? Comment { get; set; }
}
