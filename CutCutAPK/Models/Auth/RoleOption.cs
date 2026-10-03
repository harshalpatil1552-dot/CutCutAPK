namespace CutCutAPK.Models.Auth;

/// <summary>A role a user may pick during self-registration, with its display title/description
/// and a small glyph shown on the role card.</summary>
public sealed record RoleOption(RoleId Id, string Title, string Description, string Icon);

/// <summary>Mirrors the web app's role.model.ts SELF_REGISTERABLE_ROLES — Admin is provisioned out
/// of band and never offered here.</summary>
public static class SelfRegisterableRoles
{
    public static readonly IReadOnlyList<RoleOption> All = new[]
    {
        new RoleOption(RoleId.Customer, "Customer", "book appointments", "\U0001F4C5"),
        new RoleOption(RoleId.SalonStaff, "Salon Staff", "work at a salon", "\U0001F464"),
        new RoleOption(RoleId.SalonOwner, "Salon Owner", "manage a salon", "\U0001F48E"),
    };
}
