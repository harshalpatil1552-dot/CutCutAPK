namespace CutCutAPK.Models.Catalog;

/// <summary>Mirrors CutCut.API Models/Dtos/Services/ServiceResponseDto.cs — one service a salon
/// offers (Haircut, Beard Trim, etc). Lives under Models/Catalog rather than Models/Services purely
/// to avoid the folder name colliding with Services/, the app's own service layer — same reasoning
/// as the web app naming this "CatalogItem" instead of "Service".</summary>
public sealed class ServiceResponseDto
{
    public int ServiceId { get; set; }

    public int SalonId { get; set; }

    public string Name { get; set; } = string.Empty;

    public short DurationMinutes { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}
