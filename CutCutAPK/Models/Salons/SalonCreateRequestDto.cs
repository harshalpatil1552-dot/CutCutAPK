namespace CutCutAPK.Models.Salons;

/// <summary>Mirrors CutCut.API Models/Dtos/Salons/SalonCreateRequestDto.cs — POST /salons body.
/// OwnerUserId is taken from the JWT server-side, never sent from the client.</summary>
public sealed class SalonCreateRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? AddressLine { get; set; }

    public string? City { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    /// <summary>"HH:mm:ss" — see SalonResponseDto.OpeningTime for why this is a string.</summary>
    public string? OpeningTime { get; set; }

    public string? ClosingTime { get; set; }
}
