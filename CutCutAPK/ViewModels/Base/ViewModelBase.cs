using CommunityToolkit.Mvvm.ComponentModel;

namespace CutCutAPK.ViewModels.Base;

/// <summary>Common busy-state/error-state plumbing shared by every screen's ViewModel.</summary>
public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;
}
