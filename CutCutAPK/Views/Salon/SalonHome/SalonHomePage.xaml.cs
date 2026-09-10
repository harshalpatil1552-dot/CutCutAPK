using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon.SalonHome;

public partial class SalonHomePage : ContentPage
{
    private readonly SalonHomeViewModel _viewModel;

    public SalonHomePage(SalonHomeViewModel viewModel)
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
