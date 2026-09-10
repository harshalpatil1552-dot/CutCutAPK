using CutCutAPK.Models.Bookings;

namespace CutCutAPK.Models.Payments;

/// <summary>Mirrors CutCut.API Models/Dtos/Payments/PaymentConfirmResponseDto.cs.</summary>
public sealed class PaymentConfirmResponseDto
{
    public int PaymentId { get; set; }

    public BookingResponseDto Booking { get; set; } = null!;

    /// <summary>The OTP the customer will read out to salon staff at check-in.</summary>
    public OtpResponseDto Otp { get; set; } = null!;
}
