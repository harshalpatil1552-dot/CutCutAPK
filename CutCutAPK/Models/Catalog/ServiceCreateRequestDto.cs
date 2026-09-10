namespace CutCutAPK.Models.Catalog;

/// <summary>Mirrors CutCut.API Models/Dtos/Services/ServiceCreateRequestDto.cs — POST /salons/{id}/services body.</summary>
public sealed class ServiceCreateRequestDto
{
    public string Name { get; set; } = string.Empty;

    public short DurationMinutes { get; set; }

    public decimal Price { get; set; }
}
