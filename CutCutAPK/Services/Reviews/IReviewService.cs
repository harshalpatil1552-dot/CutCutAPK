using CutCutAPK.Models.Reviews;

namespace CutCutAPK.Services.Reviews;

/// <summary>Mirrors the web app's ReviewService.</summary>
public interface IReviewService
{
    Task<ReviewResponseDto> CreateAsync(ReviewCreateRequestDto request, CancellationToken cancellationToken = default);

    Task<List<ReviewResponseDto>> GetBySalonAsync(int salonId, CancellationToken cancellationToken = default);
}
