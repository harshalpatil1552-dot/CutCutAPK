using CutCutAPK.Models.Catalog;
using CutCutAPK.Services.Api;

namespace CutCutAPK.Services.Catalog;

/// <inheritdoc cref="ICatalogService" />
public sealed class CatalogService : ICatalogService
{
    private readonly IApiClient _apiClient;

    public CatalogService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<List<ServiceResponseDto>> GetBySalonAsync(int salonId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<List<ServiceResponseDto>>($"salons/{salonId}/services", cancellationToken);

    public Task<ServiceResponseDto> GetByIdAsync(int serviceId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<ServiceResponseDto>($"services/{serviceId}", cancellationToken);

    public Task<ServiceResponseDto> CreateAsync(int salonId, ServiceCreateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<ServiceResponseDto>($"salons/{salonId}/services", request, cancellationToken);

    public Task<ServiceResponseDto> UpdateAsync(int serviceId, ServiceUpdateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PutAsync<ServiceResponseDto>($"services/{serviceId}", request, cancellationToken);
}
