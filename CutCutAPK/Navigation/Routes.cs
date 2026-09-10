namespace CutCutAPK.Navigation;

/// <summary>Shell route names, kept in one place so page registration (AppShell.xaml.cs) and
/// navigation calls (NavigationService, page code-behind) can't drift out of sync.</summary>
public static class Routes
{
    public const string Login = "login";
    public const string Register = "register";

    // Superseded by the routes below (Customer/Admin now land on SalonSearch, SalonStaff/
    // SalonOwner on SalonHome) but left registered — see RoleRouting — so nothing that could
    // still reference them by route name breaks.
    public const string CustomerDashboard = "customerdashboard";
    public const string SalonDashboard = "salondashboard";

    // -- Customer area (mirrors the web app's features/customer/customer.routes.ts) --
    public const string SalonSearch = "salonsearch";
    public const string SalonDetail = "salondetail";
    public const string BookingCreate = "bookingcreate";
    public const string BookingDetail = "bookingdetail";
    public const string MyBookings = "mybookings";

    // -- Salon area (mirrors the web app's features/salon/salon.routes.ts + salon-shell gating) --
    public const string SalonHome = "salonhome";
    public const string SalonCreate = "createsalon";
    public const string SalonServices = "salonservices";
    public const string SalonSettings = "salonsettings";
    public const string SalonStaff = "salonstaff";
    public const string TodayAppointments = "todayappointments";
}
