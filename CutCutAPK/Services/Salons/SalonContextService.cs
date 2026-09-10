using CutCutAPK.Models.Salons;

namespace CutCutAPK.Services.Salons;

/// <inheritdoc cref="ISalonContextService" />
public sealed class SalonContextService : ISalonContextService
{
    private readonly ISalonService _salonService;
    private readonly List<SalonResponseDto> _mySalons = new();

    public SalonContextService(ISalonService salonService)
    {
        _salonService = salonService;
    }

    public IReadOnlyList<SalonResponseDto> MySalons => _mySalons;

    public SalonResponseDto? SelectedSalon { get; private set; }

    public bool Loaded { get; private set; }

    public async Task<IReadOnlyList<SalonResponseDto>> LoadMineAsync(CancellationToken cancellationToken = default)
    {
        var salons = await _salonService.GetMineAsync(cancellationToken);

        _mySalons.Clear();
        _mySalons.AddRange(salons);
        Loaded = true;

        if (_mySalons.Count == 1)
        {
            SelectedSalon = _mySalons[0];
        }

        return _mySalons;
    }

    public void SelectSalon(int salonId)
    {
        SelectedSalon = _mySalons.FirstOrDefault(salon => salon.SalonId == salonId);
    }

    public void UpsertSalon(SalonResponseDto salon)
    {
        var index = _mySalons.FindIndex(item => item.SalonId == salon.SalonId);

        if (index == -1)
        {
            _mySalons.Add(salon);
        }
        else
        {
            _mySalons[index] = salon;
        }

        SelectedSalon = salon;
    }

    public void Reset()
    {
        _mySalons.Clear();
        SelectedSalon = null;
        Loaded = false;
    }
}
