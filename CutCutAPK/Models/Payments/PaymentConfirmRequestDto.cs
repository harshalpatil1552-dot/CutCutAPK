namespace CutCutAPK.Models.Payments;

/// <summary>Mirrors CutCut.API Models/Dtos/Payments/PaymentConfirmRequestDto.cs — POST /payments/confirm body.
/// Simulates the gateway callback confirming a successful charge (no real gateway is wired up).</summary>
public sealed class PaymentConfirmRequestDto
{
    public string GatewayOrderId { get; set; } = string.Empty;

    public string GatewayPaymentId { get; set; } = string.Empty;
}
