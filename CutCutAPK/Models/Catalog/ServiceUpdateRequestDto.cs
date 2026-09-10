namespace CutCutAPK.Models.Catalog;

/// <summary>Mirrors CutCut.API Models/Dtos/Services/ServiceUpdateRequestDto.cs — PUT /services/{id} body.</summary>
public sealed class ServiceUpdateRequestDto
{
    public string Name { get; set; } = string.Empty;

    public short DurationMinutes { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}
