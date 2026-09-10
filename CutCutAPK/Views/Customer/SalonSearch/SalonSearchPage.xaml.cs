using CutCutAPK.ViewModels.Customer;

namespace CutCutAPK.Views.Customer.SalonSearch;

public partial class SalonSearchPage : ContentPage
{
    private readonly SalonSearchViewModel _viewModel;

    public SalonSearchPage(SalonSearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private void OnCitySearchCompleted(object? sender, EventArgs e)
    {
        if (_viewModel.SearchByCityCommand.CanExecute(null))
        {
            _viewModel.SearchByCityCommand.Execute(null);
        }
    }
}
