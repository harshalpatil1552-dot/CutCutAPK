using CutCutAPK.Common.Constants;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Api;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Bookings;
using CutCutAPK.Services.Catalog;
using CutCutAPK.Services.Http;
using CutCutAPK.Services.Payments;
using CutCutAPK.Services.Reviews;
using CutCutAPK.Services.Salons;
using CutCutAPK.Services.Session;
using CutCutAPK.Services.Staff;
using CutCutAPK.ViewModels.Auth;
using CutCutAPK.ViewModels.Customer;
using CutCutAPK.ViewModels.Salon;
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
using Microsoft.Extensions.Logging;

namespace CutCutAPK
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            RegisterServices(builder.Services);
            RegisterViewModels(builder.Services);
            RegisterViews(builder.Services);

            return builder.Build();
        }

        /// <summary>Session, navigation, API-wrapper and network plumbing — one instance for the
        /// app's lifetime. The domain services (Salon/Catalog/Booking/Review/Staff/Payment) are
        /// stateless HTTP wrappers around IApiClient, same reasoning as IApiClient itself being a
        /// singleton; ISalonContextService is the one exception that actually holds state (see its
        /// own remarks) but is still app-lifetime for the same reason IAuthService is.</summary>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ISessionStore, SecureSessionStore>();
            services.AddSingleton<ISessionExpiryNotifier, SessionExpiryNotifier>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransient<AuthHeaderHandler>();
            services
                .AddHttpClient(ApiConstants.HttpClientName, client =>
                {
                    client.BaseAddress = new Uri(ApiConstants.BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(ApiConstants.TimeoutSeconds);
                })
                .ConfigurePrimaryHttpMessageHandler(DevCertHandler.Create)
                .AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddSingleton<IApiClient, ApiClient>();

            services.AddSingleton<ISalonService, SalonService>();
            services.AddSingleton<ISalonContextService, SalonContextService>();
            services.AddSingleton<ICatalogService, CatalogService>();
            services.AddSingleton<IBookingService, BookingService>();
            services.AddSingleton<IReviewService, ReviewService>();
            services.AddSingleton<IStaffService, StaffService>();
            services.AddSingleton<IPaymentService, PaymentService>();
        }

        /// <summary>ViewModels are transient: Shell can reuse a page instance across visits (see
        /// LoginPage's OnAppearing reset), but each one still gets DI-resolved fresh per page
        /// construction rather than carrying state from a previous, unrelated navigation.</summary>
        private static void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<CustomerDashboardViewModel>();
            services.AddTransient<SalonDashboardViewModel>();

            services.AddTransient<SalonSearchViewModel>();
            services.AddTransient<SalonDetailViewModel>();
            services.AddTransient<BookingCreateViewModel>();
            services.AddTransient<BookingDetailViewModel>();
            services.AddTransient<MyBookingsViewModel>();

            services.AddTransient<SalonHomeViewModel>();
            services.AddTransient<SalonCreateViewModel>();
            services.AddTransient<SalonServicesViewModel>();
            services.AddTransient<SalonSettingsViewModel>();
            services.AddTransient<SalonStaffViewModel>();
            services.AddTransient<TodayAppointmentsViewModel>();

            // TodayAppointmentRowViewModel is intentionally not registered here — it's created
            // directly by TodayAppointmentsViewModel, one per booking, not resolved via DI.
        }

        private static void RegisterViews(IServiceCollection services)
        {
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<CustomerDashboardPage>();
            services.AddTransient<SalonDashboardPage>();

            services.AddTransient<SalonSearchPage>();
            services.AddTransient<SalonDetailPage>();
            services.AddTransient<BookingCreatePage>();
            services.AddTransient<BookingDetailPage>();
            services.AddTransient<MyBookingsPage>();

            services.AddTransient<SalonHomePage>();
            services.AddTransient<SalonCreatePage>();
            services.AddTransient<SalonServicesPage>();
            services.AddTransient<SalonSettingsPage>();
            services.AddTransient<SalonStaffPage>();
            services.AddTransient<TodayAppointmentsPage>();
        }
    }
}
