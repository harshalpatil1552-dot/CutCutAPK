using CutCutAPK.Models.Salons;

namespace CutCutAPK.Services.Salons;

/// <summary>
/// Holds "which salon is the signed-in staff/owner currently managing" — mirrors the web app's
/// SalonContextService, used by every salon-side page (today's appointments, services, staff,
/// settings) instead of each one re-fetching /salons/mine and re-deriving the selection itself.
/// A plain mutable singleton rather than an observable one: each salon page re-reads it fresh in
/// its constructor (MAUI pages are recreated per navigation, unlike Angular's persistent
/// component tree), so there's no cross-page reactivity to wire up.
/// </summary>
public interface ISalonContextService
{
    IReadOnlyList<SalonResponseDto> MySalons { get; }

    SalonResponseDto? SelectedSalon { get; }

    bool Loaded { get; }

    /// <summary>Loads /salons/mine. If the caller manages exactly one salon it's selected
    /// automatically, same as the web app.</summary>
    Task<IReadOnlyList<SalonResponseDto>> LoadMineAsync(CancellationToken cancellationToken = default);

    void SelectSalon(int salonId);

    /// <summary>Called after creating a salon, or updating the selected one, so the rest of the
    /// salon area reflects it immediately.</summary>
    void UpsertSalon(SalonResponseDto salon);

    void Reset();
}
