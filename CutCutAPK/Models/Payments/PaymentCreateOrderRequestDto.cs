namespace CutCutAPK.Models.Payments;

/// <summary>Mirrors CutCut.API Models/Dtos/Payments/PaymentCreateOrderRequestDto.cs — POST /payments/orders body.</summary>
public sealed class PaymentCreateOrderRequestDto
{
    public int BookingId { get; set; }
}
