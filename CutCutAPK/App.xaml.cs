using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;

namespace CutCutAPK
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;

        public App(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // AppShell always starts on the login route (see AppShell.xaml); once the window is
            // up we hydrate the persisted session and — mirroring the web app's guestGuard —
            // skip straight past login for a user who's already signed in.
            window.Created += async (_, _) => await RestoreSessionAsync();

            return window;
        }

        private async Task RestoreSessionAsync()
        {
            // if restore fails, go to login instead of crashing the app
            try
            {
                await _authService.InitializeAsync();

                if (_authService.IsAuthenticated)
                {
                    await Shell.Current.GoToAsync($"//{RoleRouting.HomeRoute(_authService.CurrentUser?.RoleName)}");
                }
            }
            catch (Exception)
            {
                await _authService.LogoutAsync();
            }
        }
    }
}
