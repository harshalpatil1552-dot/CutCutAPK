using CutCutAPK.Models.Payments;

namespace CutCutAPK.Services.Payments;

/// <summary>
/// Mirrors the web app's PaymentService. No real payment gateway is integrated on the backend yet
/// (see the API's PaymentService.cs remarks) — confirm() stands in for what would otherwise be a
/// gateway SDK checkout callback.
/// </summary>
public interface IPaymentService
{
    Task<PaymentOrderResponseDto> CreateOrderAsync(int bookingId, CancellationToken cancellationToken = default);

    Task<PaymentConfirmResponseDto> ConfirmAsync(string gatewayOrderId, string gatewayPaymentId, CancellationToken cancellationToken = default);
}
