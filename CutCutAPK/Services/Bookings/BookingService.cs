using CutCutAPK.Models.Bookings;
using CutCutAPK.Services.Api;

namespace CutCutAPK.Services.Bookings;

/// <inheritdoc cref="IBookingService" />
public sealed class BookingService : IBookingService
{
    private const string BaseRoute = "bookings";

    private readonly IApiClient _apiClient;

    public BookingService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<BookingResponseDto> CreateAsync(BookingCreateRequestDto request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<BookingResponseDto>(BaseRoute, request, cancellationToken);

    public Task<List<BookingResponseDto>> GetMineAsync(CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<List<BookingResponseDto>>($"{BaseRoute}/mine", cancellationToken);

    public Task<BookingResponseDto> GetByIdAsync(int bookingId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<BookingResponseDto>($"{BaseRoute}/{bookingId}", cancellationToken);

    public Task<List<BookingResponseDto>> GetSalonTodayAsync(int salonId, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<List<BookingResponseDto>>($"{BaseRoute}/salon/{salonId}/today", cancellationToken);

    public Task<OtpResponseDto> RegenerateOtpAsync(int bookingId, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<OtpResponseDto>($"{BaseRoute}/{bookingId}/otp/regenerate", null, cancellationToken);

    public Task<BookingResponseDto> CheckInAsync(int bookingId, string otpCode, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<BookingResponseDto>(
            $"{BaseRoute}/{bookingId}/checkin", new BookingCheckInRequestDto { OtpCode = otpCode }, cancellationToken);

    public Task<BookingResponseDto> StartAsync(int bookingId, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<BookingResponseDto>($"{BaseRoute}/{bookingId}/start", null, cancellationToken);

    public Task<BookingResponseDto> CompleteAsync(int bookingId, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<BookingResponseDto>($"{BaseRoute}/{bookingId}/complete", null, cancellationToken);

    public Task<BookingResponseDto> CancelAsync(int bookingId, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<BookingResponseDto>($"{BaseRoute}/{bookingId}/cancel", null, cancellationToken);
}
