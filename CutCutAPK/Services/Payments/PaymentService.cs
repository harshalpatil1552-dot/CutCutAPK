using CutCutAPK.Models.Payments;
using CutCutAPK.Services.Api;

namespace CutCutAPK.Services.Payments;

/// <inheritdoc cref="IPaymentService" />
public sealed class PaymentService : IPaymentService
{
    private readonly IApiClient _apiClient;

    public PaymentService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<PaymentOrderResponseDto> CreateOrderAsync(int bookingId, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<PaymentOrderResponseDto>(
            "payments/orders", new PaymentCreateOrderRequestDto { BookingId = bookingId }, cancellationToken);

    public Task<PaymentConfirmResponseDto> ConfirmAsync(string gatewayOrderId, string gatewayPaymentId, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<PaymentConfirmResponseDto>(
            "payments/confirm",
            new PaymentConfirmRequestDto { GatewayOrderId = gatewayOrderId, GatewayPaymentId = gatewayPaymentId },
            cancellationToken);
}
