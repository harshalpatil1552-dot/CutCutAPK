using CommunityToolkit.Mvvm.ComponentModel;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>One day pill in BookingCreate's date strip.</summary>
public sealed partial class BookingDateOption : ObservableObject
{
    public required DateOnly Date { get; init; }

    public required string DayNumber { get; init; }

    public required string DayName { get; init; }

    [ObservableProperty]
    private bool isSelected;
}
