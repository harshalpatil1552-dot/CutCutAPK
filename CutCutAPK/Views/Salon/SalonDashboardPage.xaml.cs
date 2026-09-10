using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon;

public partial class SalonDashboardPage : ContentPage
{
    public SalonDashboardPage(SalonDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
