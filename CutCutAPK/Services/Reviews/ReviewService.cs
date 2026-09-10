using CutCutAPK.Models.Reviews;
using CutCutAPK.Services.Api;

namespace CutCutAPK.Services.Reviews;

/// <inheritdoc cref="IReviewService" />
public sealed class ReviewService : IReviewService
{
    private readonly IApiClient _apiClient;

    public ReviewService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<ReviewResponseDto> CreateAsync(ReviewCreateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<ReviewResponseDto>("reviews", request, cancellationToken);

    public Task<List<ReviewResponseDto>> GetBySalonAsync(int salonId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<List<ReviewResponseDto>>($"salons/{salonId}/reviews", cancellationToken);
}
