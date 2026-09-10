namespace CutCutAPK.Models.Auth;

/// <summary>Mirrors CutCut.API Models/Dtos/Auth/RegisterRequestDto.cs — POST /api/v1/auth/register body.</summary>
public sealed class RegisterRequestDto
{
    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string Password { get; set; } = string.Empty;

    /// <summary>1 = Customer, 2 = SalonStaff, 3 = SalonOwner — Admin (4) is provisioned out of
    /// band and excluded from self-registration, same as the web app.</summary>
    public byte RoleId { get; set; } = (byte)Auth.RoleId.Customer;
}
