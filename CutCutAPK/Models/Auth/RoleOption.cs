namespace CutCutAPK.Models.Auth;

/// <summary>A role a user may pick during self-registration, with its display label.</summary>
public sealed record RoleOption(RoleId Id, string Label);

/// <summary>Mirrors the web app's role.model.ts SELF_REGISTERABLE_ROLES — Admin is provisioned out
/// of band and never offered here.</summary>
public static class SelfRegisterableRoles
{
    public static readonly IReadOnlyList<RoleOption> All = new[]
    {
        new RoleOption(RoleId.Customer, "Customer — book appointments"),
        new RoleOption(RoleId.SalonStaff, "Salon Staff — work at a salon"),
        new RoleOption(RoleId.SalonOwner, "Salon Owner — manage a salon"),
    };
}
