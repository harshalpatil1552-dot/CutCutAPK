namespace CutCutAPK.Models.Salons;

/// <summary>Query parameters for GET /salons ("search nearby salons") — mirrors the web app's
/// SalonSearchParams. Built into a query string by SalonService rather than sent as a JSON body,
/// since this is a GET.</summary>
public sealed class SalonSearchParams
{
    public string? City { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public double? RadiusKm { get; set; }
}
