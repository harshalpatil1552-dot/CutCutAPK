using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Models.Bookings;
using CutCutAPK.Models.Payments;
using CutCutAPK.Models.Reviews;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.Services.Bookings;
using CutCutAPK.Services.Payments;
using CutCutAPK.Services.Reviews;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Customer;

/// <summary>Mirrors features/customer/booking-detail/booking-detail.ts — the single most stateful
/// screen in the app: simulated payment, one-time check-in code, and post-completion review, all
/// gated by the booking's current status.</summary>
public sealed partial class BookingDetailViewModel : CustomerAreaViewModelBase, IQueryAttributable
{
    private readonly IBookingService _bookingService;
    private readonly IPaymentService _paymentService;
    private readonly IReviewService _reviewService;

    private int _bookingId;

    public BookingDetailViewModel(
        IBookingService bookingService,
        IPaymentService paymentService,
        IReviewService reviewService,
        IAuthService authService,
        INavigationService navigationService)
        : base(authService, navigationService)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
        _reviewService = reviewService;
    }

    [ObservableProperty]
    private BookingResponseDto? booking;

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string? infoMessage;

    // ---------------- Payment ----------------

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyActionBusy))]
    private PaymentOrderResponseDto? pendingOrder;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyActionBusy))]
    private bool isPaying;

    // ---------------- Check-in code ----------------

    /// <summary>Only ever known right after generate/regenerate — never re-fetchable, matching the
    /// backend (it hands the OTP back once, same as the web app).</summary>
    [ObservableProperty]
    private string? otpCode;

    [ObservableProperty]
    private DateTime? otpExpiresAtUtc;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyActionBusy))]
    private bool isFetchingOtp;

    // ---------------- Review ----------------

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowRatingPrompt))]
    private bool hasExistingReview;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowRatingPrompt))]
    private bool showReviewForm;

    [ObservableProperty]
    private int reviewRating = 5;

    [ObservableProperty]
    private string reviewComment = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyActionBusy))]
    private bool isSubmittingReview;

    /// <summary>Named distinctly from the inherited <see cref="ViewModelBase.IsBusy"/> (which this
    /// screen doesn't use) so the source-generated setter for the base property is never shadowed.</summary>
    public bool IsAnyActionBusy => IsPaying || IsFetchingOtp || IsSubmittingReview;

    /// <summary>Case 2 of the review card's three-way branch: no review yet, and the form hasn't
    /// been opened — mirrors the web app's `@else if (!showReviewForm())` prompt.</summary>
    public bool ShowRatingPrompt => !HasExistingReview && !ShowReviewForm;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("bookingId", out var value) && int.TryParse(value?.ToString(), out var bookingId))
        {
            _bookingId = bookingId;
        }
    }

    public async Task LoadAsync()
    {
        ErrorMessage = null;
        InfoMessage = null;
        IsLoading = true;

        try
        {
            Booking = await _bookingService.GetByIdAsync(_bookingId);

            if (Booking?.BookingStatusId == BookingStatusId.Completed)
            {
                await CheckExistingReviewAsync(Booking.SalonId);
            }
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task CheckExistingReviewAsync(int salonId)
    {
        try
        {
            var reviews = await _reviewService.GetBySalonAsync(salonId);
            HasExistingReview = reviews.Any(review => review.BookingId == _bookingId);
        }
        catch (Exception)
        {
            // Same as the web app: a failure here just leaves the "rate this salon" prompt showing.
        }
    }

    [RelayCommand]
    private async Task StartPaymentAsync()
    {
        ErrorMessage = null;
        IsPaying = true;

        try
        {
            PendingOrder = await _paymentService.CreateOrderAsync(_bookingId);
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsPaying = false;
        }
    }

    /// <summary>Stands in for a real gateway's success callback — see PaymentService.cs remarks on
    /// the backend; no real gateway is wired up yet.</summary>
    [RelayCommand]
    private async Task SimulatePaymentSuccessAsync()
    {
        var order = PendingOrder;
        if (order is null)
        {
            return;
        }

        ErrorMessage = null;
        IsPaying = true;

        try
        {
            var result = await _paymentService.ConfirmAsync(order.GatewayOrderId, $"SIMPAY-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");

            PendingOrder = null;
            Booking = result.Booking;
            OtpCode = result.Otp.OtpCode;
            OtpExpiresAtUtc = result.Otp.ExpiresAtUtc;
            InfoMessage = "Payment confirmed! Read the code below out to salon staff at check-in.";
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsPaying = false;
        }
    }

    [RelayCommand]
    private async Task FetchCheckInCodeAsync()
    {
        ErrorMessage = null;
        IsFetchingOtp = true;

        try
        {
            var otp = await _bookingService.RegenerateOtpAsync(_bookingId);
            OtpCode = otp.OtpCode;
            OtpExpiresAtUtc = otp.ExpiresAtUtc;
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsFetchingOtp = false;
        }
    }

    [RelayCommand]
    private async Task CancelBookingAsync()
    {
        ErrorMessage = null;

        try
        {
            Booking = await _bookingService.CancelAsync(_bookingId);
            OtpCode = null;
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
    }

    [RelayCommand]
    private void BeginReview() => ShowReviewForm = true;

    [RelayCommand]
    private void SetRating(string starValue)
    {
        if (int.TryParse(starValue, out var rating))
        {
            ReviewRating = rating;
        }
    }

    [RelayCommand]
    private async Task SubmitReviewAsync()
    {
        ErrorMessage = null;
        IsSubmittingReview = true;

        try
        {
            await _reviewService.CreateAsync(new ReviewCreateRequestDto
            {
                BookingId = _bookingId,
                Rating = (byte)ReviewRating,
                Comment = string.IsNullOrWhiteSpace(ReviewComment) ? null : ReviewComment.Trim(),
            });

            HasExistingReview = true;
            ShowReviewForm = false;
            InfoMessage = "Thanks for your feedback!";
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsSubmittingReview = false;
        }
    }
}
