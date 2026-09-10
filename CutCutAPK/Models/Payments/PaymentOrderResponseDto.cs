namespace CutCutAPK.Models.Payments;

/// <summary>Mirrors CutCut.API Models/Dtos/Payments/PaymentOrderResponseDto.cs. No real payment
/// gateway is integrated on the backend yet — GatewayOrderId stands in for what a real gateway
/// would hand back, same as the web app.</summary>
public sealed class PaymentOrderResponseDto
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public string GatewayOrderId { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}
