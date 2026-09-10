namespace CutCutAPK.Models.Salons;

/// <summary>Mirrors CutCut.API Models/Dtos/Salons/SalonResponseDto.cs.</summary>
public sealed class SalonResponseDto
{
    public int SalonId { get; set; }

    public int OwnerUserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? AddressLine { get; set; }

    public string? City { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    /// <summary>Raw "HH:mm:ss" wire format, kept as a string rather than parsed client-side — same
    /// choice the web app makes (salon.model.ts types these as string | null) since the screens
    /// only ever display or round-trip them, never compute with them.</summary>
    public string? OpeningTime { get; set; }

    public string? ClosingTime { get; set; }

    public decimal AvgRating { get; set; }

    public bool HasPhoto { get; set; }

    public bool IsActive { get; set; }

    /// <summary>Populated only when the caller supplied their own lat/lng in a nearby-search request.</summary>
    public double? DistanceKm { get; set; }
}
