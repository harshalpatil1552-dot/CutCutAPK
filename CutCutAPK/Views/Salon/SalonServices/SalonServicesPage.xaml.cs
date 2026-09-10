using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon.SalonServices;

public partial class SalonServicesPage : ContentPage
{
    private readonly SalonServicesViewModel _viewModel;

    public SalonServicesPage(SalonServicesViewModel viewModel)
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
