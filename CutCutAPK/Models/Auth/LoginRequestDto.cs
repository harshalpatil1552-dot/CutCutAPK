namespace CutCutAPK.Models.Auth;

/// <summary>Mirrors CutCut.API Models/Dtos/Auth/LoginRequestDto.cs — POST /api/v1/auth/login body.
/// Accepts either the registered phone or email.</summary>
public sealed class LoginRequestDto
{
    public string PhoneOrEmail { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
