using CutCutAPK.ViewModels.Customer;

namespace CutCutAPK.Views.Customer.MyBookings;

public partial class MyBookingsPage : ContentPage
{
    private readonly MyBookingsViewModel _viewModel;

    public MyBookingsPage(MyBookingsViewModel viewModel)
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
