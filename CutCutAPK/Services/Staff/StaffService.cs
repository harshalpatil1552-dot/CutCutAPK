using CutCutAPK.Models.Staff;
using CutCutAPK.Services.Api;

namespace CutCutAPK.Services.Staff;

/// <inheritdoc cref="IStaffService" />
public sealed class StaffService : IStaffService
{
    private readonly IApiClient _apiClient;

    public StaffService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<List<StaffResponseDto>> GetBySalonAsync(int salonId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<List<StaffResponseDto>>($"salons/{salonId}/staff", cancellationToken);

    public Task<StaffResponseDto> AddAsync(int salonId, StaffCreateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<StaffResponseDto>($"salons/{salonId}/staff", request, cancellationToken);

    public Task<StaffLookupResponseDto> LookupByPhoneAsync(string phone, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<StaffLookupResponseDto>($"staff/lookup?phone={Uri.EscapeDataString(phone)}", cancellationToken);
}
