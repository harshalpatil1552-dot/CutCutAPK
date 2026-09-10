using CutCutAPK.ViewModels.Customer;

namespace CutCutAPK.Views.Customer;

public partial class CustomerDashboardPage : ContentPage
{
    public CustomerDashboardPage(CustomerDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
