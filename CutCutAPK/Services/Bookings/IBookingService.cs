using CutCutAPK.Models.Bookings;

namespace CutCutAPK.Services.Bookings;

/// <summary>Mirrors the web app's BookingService — the full booking lifecycle.</summary>
public interface IBookingService
{
    Task<BookingResponseDto> CreateAsync(BookingCreateRequestDto request, CancellationToken cancellationToken = default);

    Task<List<BookingResponseDto>> GetMineAsync(CancellationToken cancellationToken = default);

    Task<BookingResponseDto> GetByIdAsync(int bookingId, CancellationToken cancellationToken = default);

    Task<List<BookingResponseDto>> GetSalonTodayAsync(int salonId, CancellationToken cancellationToken = default);

    Task<OtpResponseDto> RegenerateOtpAsync(int bookingId, CancellationToken cancellationToken = default);

    Task<BookingResponseDto> CheckInAsync(int bookingId, string otpCode, CancellationToken cancellationToken = default);

    Task<BookingResponseDto> StartAsync(int bookingId, CancellationToken cancellationToken = default);

    Task<BookingResponseDto> CompleteAsync(int bookingId, CancellationToken cancellationToken = default);

    Task<BookingResponseDto> CancelAsync(int bookingId, CancellationToken cancellationToken = default);
}
