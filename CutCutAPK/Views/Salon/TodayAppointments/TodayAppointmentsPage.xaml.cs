using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon.TodayAppointments;

public partial class TodayAppointmentsPage : ContentPage
{
    private readonly TodayAppointmentsViewModel _viewModel;

    public TodayAppointmentsPage(TodayAppointmentsViewModel viewModel)
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
