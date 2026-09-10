using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon.SalonSettings;

public partial class SalonSettingsPage : ContentPage
{
    private readonly SalonSettingsViewModel _viewModel;

    public SalonSettingsPage(SalonSettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
