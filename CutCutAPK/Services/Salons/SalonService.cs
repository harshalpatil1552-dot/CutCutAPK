using System.Globalization;
using CutCutAPK.Models.Salons;
using CutCutAPK.Services.Api;

namespace CutCutAPK.Services.Salons;

/// <inheritdoc cref="ISalonService" />
public sealed class SalonService : ISalonService
{
    private const string BaseRoute = "salons";

    private readonly IApiClient _apiClient;

    public SalonService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<List<SalonResponseDto>> SearchAsync(SalonSearchParams parameters, CancellationToken cancellationToken = default)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(parameters.City))
        {
            query.Add($"city={Uri.EscapeDataString(parameters.City)}");
        }

        if (parameters.Latitude is { } latitude)
        {
            query.Add($"latitude={latitude.ToString(CultureInfo.InvariantCulture)}");
        }

        if (parameters.Longitude is { } longitude)
        {
            query.Add($"longitude={longitude.ToString(CultureInfo.InvariantCulture)}");
        }

        if (parameters.RadiusKm is { } radiusKm)
        {
            query.Add($"radiusKm={radiusKm.ToString(CultureInfo.InvariantCulture)}");
        }

        var route = query.Count > 0 ? $"{BaseRoute}?{string.Join('&', query)}" : BaseRoute;
        return _apiClient.GetAsync<List<SalonResponseDto>>(route, cancellationToken);
    }

    public Task<SalonResponseDto> GetByIdAsync(int salonId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<SalonResponseDto>($"{BaseRoute}/{salonId}", cancellationToken);

    public Task<List<SalonResponseDto>> GetMineAsync(CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<List<SalonResponseDto>>($"{BaseRoute}/mine", cancellationToken);

    public Task<SalonResponseDto> CreateAsync(SalonCreateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<SalonResponseDto>(BaseRoute, request, cancellationToken);

    public Task<SalonResponseDto> UpdateAsync(int salonId, SalonUpdateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PutAsync<SalonResponseDto>($"{BaseRoute}/{salonId}", request, cancellationToken);
}
