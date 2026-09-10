using CutCutAPK.ViewModels.Salon;

namespace CutCutAPK.Views.Salon.SalonCreate;

public partial class SalonCreatePage : ContentPage
{
    public SalonCreatePage(SalonCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
