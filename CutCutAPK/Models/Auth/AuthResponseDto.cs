namespace CutCutAPK.Models.Auth;

/// <summary>Mirrors CutCut.API Models/Dtos/Auth/AuthResponseDto.cs — returned by both Register and Login.</summary>
public sealed class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public UserProfileDto User { get; set; } = null!;
}
