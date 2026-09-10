using CutCutAPK.Models.Staff;

namespace CutCutAPK.Services.Staff;

/// <summary>Mirrors the web app's StaffService — a SalonOwner managing who works at their salon.</summary>
public interface IStaffService
{
    Task<List<StaffResponseDto>> GetBySalonAsync(int salonId, CancellationToken cancellationToken = default);

    Task<StaffResponseDto> AddAsync(int salonId, StaffCreateRequestDto request, CancellationToken cancellationToken = default);

    Task<StaffLookupResponseDto> LookupByPhoneAsync(string phone, CancellationToken cancellationToken = default);
}
