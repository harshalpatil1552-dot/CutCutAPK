namespace CutCutAPK.Models.Salons;

/// <summary>Mirrors CutCut.API Models/Dtos/Salons/SalonUpdateRequestDto.cs — PUT /salons/{id} body.</summary>
public sealed class SalonUpdateRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? AddressLine { get; set; }

    public string? City { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    /// <summary>"HH:mm:ss" — see SalonResponseDto.OpeningTime for why this is a string.</summary>
    public string? OpeningTime { get; set; }

    public string? ClosingTime { get; set; }

    public bool IsActive { get; set; } = true;
}
