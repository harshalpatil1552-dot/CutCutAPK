using CutCutAPK.ViewModels.Customer;

namespace CutCutAPK.Views.Customer.BookingDetail;

public partial class BookingDetailPage : ContentPage
{
    private readonly BookingDetailViewModel _viewModel;

    public BookingDetailPage(BookingDetailViewModel viewModel)
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
