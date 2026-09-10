using CutCutAPK.Navigation;
using CutCutAPK.Views.Auth;
using CutCutAPK.Views.Customer;
using CutCutAPK.Views.Customer.BookingCreate;
using CutCutAPK.Views.Customer.BookingDetail;
using CutCutAPK.Views.Customer.MyBookings;
using CutCutAPK.Views.Customer.SalonDetail;
using CutCutAPK.Views.Customer.SalonSearch;
using CutCutAPK.Views.Salon;
using CutCutAPK.Views.Salon.SalonCreate;
using CutCutAPK.Views.Salon.SalonHome;
using CutCutAPK.Views.Salon.SalonServices;
using CutCutAPK.Views.Salon.SalonSettings;
using CutCutAPK.Views.Salon.SalonStaff;
using CutCutAPK.Views.Salon.TodayAppointments;

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

            // -- Customer area --
            Routing.RegisterRoute(Routes.SalonSearch, typeof(SalonSearchPage));
            Routing.RegisterRoute(Routes.SalonDetail, typeof(SalonDetailPage));
            Routing.RegisterRoute(Routes.BookingCreate, typeof(BookingCreatePage));
            Routing.RegisterRoute(Routes.BookingDetail, typeof(BookingDetailPage));
            Routing.RegisterRoute(Routes.MyBookings, typeof(MyBookingsPage));

            // -- Salon area --
            Routing.RegisterRoute(Routes.SalonHome, typeof(SalonHomePage));
            Routing.RegisterRoute(Routes.SalonCreate, typeof(SalonCreatePage));
            Routing.RegisterRoute(Routes.SalonServices, typeof(SalonServicesPage));
            Routing.RegisterRoute(Routes.SalonSettings, typeof(SalonSettingsPage));
            Routing.RegisterRoute(Routes.SalonStaff, typeof(SalonStaffPage));
            Routing.RegisterRoute(Routes.TodayAppointments, typeof(TodayAppointmentsPage));
        }
    }
}
