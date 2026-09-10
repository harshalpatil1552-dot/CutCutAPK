namespace CutCutAPK.Models.Auth;

/// <summary>Mirrors CutCut.API Common/Constants/RoleIds.cs.</summary>
public enum RoleId : byte
{
    Customer = 1,
    SalonStaff = 2,
    SalonOwner = 3,
    Admin = 4,
}

/// <summary>Mirrors CutCut.API Common/Constants/RoleNames.cs. Kept as string constants (rather than
/// an enum) because that's the exact shape the API serializes UserProfileDto.RoleName as.</summary>
public static class RoleName
{
    public const string Customer = "Customer";
    public const string SalonStaff = "SalonStaff";
    public const string SalonOwner = "SalonOwner";
    public const string Admin = "Admin";
}
