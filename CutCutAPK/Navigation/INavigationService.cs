namespace CutCutAPK.Navigation;

/// <summary>Abstracts Shell navigation behind an interface so ViewModels depend on a testable
/// seam instead of the static Shell.Current API directly.</summary>
public interface INavigationService
{
    /// <summary>Replaces the whole navigation stack with the role-appropriate home page —
    /// used after a successful login so the login screen isn't left on the back stack.</summary>
    Task NavigateToRoleHomeAsync(string? roleName);

    /// <summary>Replaces the whole navigation stack with the login page — used on logout and on
    /// session expiry.</summary>
    Task NavigateToLoginAsync();

    Task NavigateToRegisterAsync();

    /// <summary>Pops the current page — used by Register's "Already have an account? Sign in"
    /// footer link to return to the Login page still on the back stack, rather than replacing the
    /// whole navigation stack the way NavigateToLoginAsync does for logout/session-expiry.</summary>
    Task GoBackAsync();

    /// <summary>Pushes an arbitrary registered route onto the navigation stack — e.g.
    /// "salondetail?salonId=5". Used by every signed-in screen's card taps and header nav links,
    /// the mobile equivalent of the web app's [routerLink] bindings.</summary>
    Task NavigateToAsync(string route);

    /// <summary>Replaces the whole navigation stack with the given route — used when landing on
    /// the salon area's default child screen (Today's Appointments) after a salon is selected, the
    /// same way NavigateToRoleHomeAsync replaces the stack after login.</summary>
    Task NavigateToRootAsync(string route);
}
