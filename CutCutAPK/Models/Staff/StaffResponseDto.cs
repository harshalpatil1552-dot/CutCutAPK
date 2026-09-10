namespace CutCutAPK.Models.Staff;

/// <summary>Mirrors CutCut.API Models/Dtos/Staff/StaffResponseDto.cs.</summary>
public sealed class StaffResponseDto
{
    public int StaffId { get; set; }

    public int SalonId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
