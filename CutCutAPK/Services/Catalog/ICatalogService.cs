using CutCutAPK.Models.Catalog;

namespace CutCutAPK.Services.Catalog;

/// <summary>Mirrors the web app's CatalogService — wraps the Services (haircut/beard-trim/etc.)
/// endpoints. Named "Catalog" to avoid clashing with the app's own Services/ layer, same reasoning
/// the web app calls out for its own CatalogService.</summary>
public interface ICatalogService
{
    Task<List<ServiceResponseDto>> GetBySalonAsync(int salonId, CancellationToken cancellationToken = default);

    Task<ServiceResponseDto> GetByIdAsync(int serviceId, CancellationToken cancellationToken = default);

    Task<ServiceResponseDto> CreateAsync(int salonId, ServiceCreateRequestDto request, CancellationToken cancellationToken = default);

    Task<ServiceResponseDto> UpdateAsync(int serviceId, ServiceUpdateRequestDto request, CancellationToken cancellationToken = default);
}
