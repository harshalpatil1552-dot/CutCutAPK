using CutCutAPK.Models.Salons;

namespace CutCutAPK.Services.Salons;

/// <summary>Mirrors the web app's SalonService — salon search (public) and salon management
/// (SalonOwner).</summary>
public interface ISalonService
{
    Task<List<SalonResponseDto>> SearchAsync(SalonSearchParams parameters, CancellationToken cancellationToken = default);

    Task<SalonResponseDto> GetByIdAsync(int salonId, CancellationToken cancellationToken = default);

    /// <summary>Salons the caller owns (SalonOwner) or works at (SalonStaff).</summary>
    Task<List<SalonResponseDto>> GetMineAsync(CancellationToken cancellationToken = default);

    Task<SalonResponseDto> CreateAsync(SalonCreateRequestDto request, CancellationToken cancellationToken = default);

    Task<SalonResponseDto> UpdateAsync(int salonId, SalonUpdateRequestDto request, CancellationToken cancellationToken = default);
}
