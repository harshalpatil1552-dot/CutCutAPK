using CutCutAPK.ViewModels.Customer;

namespace CutCutAPK.Views.Customer.BookingCreate;

public partial class BookingCreatePage : ContentPage
{
    private readonly BookingCreateViewModel _viewModel;

    public BookingCreatePage(BookingCreateViewModel viewModel)
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
