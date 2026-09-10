using CutCutAPK.Models.Auth;

namespace CutCutAPK.Navigation;

/// <summary>Where a signed-in user lands after login, and where a session-restore at app startup
/// sends an already-signed-in user. Mirrors the web app's homeRouteForRole — one place to update
/// when real role-specific dashboards replace these placeholders.</summary>
public static class RoleRouting
{
    public static string HomeRoute(string? roleName) => roleName switch
    {
        RoleName.SalonStaff or RoleName.SalonOwner => Routes.SalonHome,
        _ => Routes.SalonSearch,
    };
}
