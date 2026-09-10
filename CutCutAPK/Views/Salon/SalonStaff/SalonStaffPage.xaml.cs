using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon.SalonStaff;

public partial class SalonStaffPage : ContentPage
{
    private readonly SalonStaffViewModel _viewModel;

    public SalonStaffPage(SalonStaffViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    /// <summary>Mirrors the web app's (keyup.enter)="lookup()" on the phone field.</summary>
    private void OnLookupPhoneCompleted(object? sender, EventArgs e)
    {
        if (_viewModel.LookupCommand.CanExecute(null))
        {
            _viewModel.LookupCommand.Execute(null);
        }
    }
}
