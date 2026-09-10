using CutCutAPK.ViewModels.Customer;

namespace CutCutAPK.Views.Customer.SalonDetail;

public partial class SalonDetailPage : ContentPage
{
    private readonly SalonDetailViewModel _viewModel;

    public SalonDetailPage(SalonDetailViewModel viewModel)
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
