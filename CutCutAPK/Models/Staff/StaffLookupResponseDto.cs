namespace CutCutAPK.Models.Staff;

/// <summary>Mirrors CutCut.API Models/Dtos/Staff/StaffLookupResponseDto.cs — response for
/// GET /staff/lookup?phone=... .</summary>
public sealed class StaffLookupResponseDto
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}
