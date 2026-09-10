namespace CutCutAPK.Models.Auth;

/// <summary>Mirrors CutCut.API Models/Dtos/Auth/UserProfileDto.cs — never carries PasswordHash or
/// other internal columns.</summary>
public sealed class UserProfileDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public byte RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;
}
