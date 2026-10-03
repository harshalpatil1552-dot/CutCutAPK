using CutCutAPK.Navigation;
using CutCutAPK.Views.Auth;
using CutCutAPK.Views.Customer;
using CutCutAPK.Views.Customer.BookingCreate;
using CutCutAPK.Views.Customer.BookingDetail;
using CutCutAPK.Views.Customer.SalonDetail;
using CutCutAPK.Views.Salon;
using CutCutAPK.Views.Salon.SalonCreate;

namespace CutCutAPK
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(Routes.Register, typeof(RegisterPage));

            // Superseded placeholders — left registered so nothing still referencing them by
            // route name breaks (see Routes.cs).
            Routing.RegisterRoute(Routes.CustomerDashboard, typeof(CustomerDashboardPage));
            Routing.RegisterRoute(Routes.SalonDashboard, typeof(SalonDashboardPage));

            // -- Customer area (the Find a salon / My bookings tab bar is in AppShell.xaml) --
            Routing.RegisterRoute(Routes.SalonDetail, typeof(SalonDetailPage));
            Routing.RegisterRoute(Routes.BookingCreate, typeof(BookingCreatePage));
            Routing.RegisterRoute(Routes.BookingDetail, typeof(BookingDetailPage));

            // -- Salon area (SalonHome and the Today/Services/Staff/Settings tab bar are in
            //    AppShell.xaml) --
            Routing.RegisterRoute(Routes.SalonCreate, typeof(SalonCreatePage));
        }
    }
}
