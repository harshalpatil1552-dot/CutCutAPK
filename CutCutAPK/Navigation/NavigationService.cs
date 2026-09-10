namespace CutCutAPK.Navigation;

/// <inheritdoc cref="INavigationService" />
public sealed class NavigationService : INavigationService
{
    public Task NavigateToRoleHomeAsync(string? roleName) =>
        Shell.Current.GoToAsync($"//{RoleRouting.HomeRoute(roleName)}");

    public Task NavigateToLoginAsync() =>
        Shell.Current.GoToAsync($"//{Routes.Login}");

    public Task NavigateToRegisterAsync() =>
        Shell.Current.GoToAsync(Routes.Register);

    public Task GoBackAsync() =>
        Shell.Current.GoToAsync("..");

    public Task NavigateToAsync(string route) =>
        Shell.Current.GoToAsync(route);

    public Task NavigateToRootAsync(string route) =>
        Shell.Current.GoToAsync($"//{route}");
}
