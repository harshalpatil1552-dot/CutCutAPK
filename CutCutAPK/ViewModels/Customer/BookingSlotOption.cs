using CommunityToolkit.Mvvm.ComponentModel;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>One time pill in BookingCreate's available-slots grid.</summary>
public sealed partial class BookingSlotOption : ObservableObject
{
    public required DateTime StartTimeUtc { get; init; }

    public required string DisplayTime { get; init; }

    public required bool IsAvailable { get; init; }

    [ObservableProperty]
    private bool isSelected;
}
