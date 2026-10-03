using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Common.Validation;
using CutCutAPK.Models.Auth;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Auth;

/// <summary>
/// Backs the registration screen. Mirrors features/auth/register/register.ts field-for-field,
/// including validator priority order (required beats length beats format, same as the order the
/// web app lists each field's `messages` object) and the password/confirmPassword cross-field
/// check from shared/validators/password-match.validator.ts.
/// </summary>
public sealed partial class RegisterViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    private bool _fullNameTouched;
    private bool _phoneTouched;
    private bool _emailTouched;
    private bool _passwordTouched;
    private bool _confirmPasswordTouched;

    public RegisterViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    public IReadOnlyList<RoleOption> RoleOptions => SelfRegisterableRoles.All;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private RoleId selectedRoleId = RoleId.Customer;

    [ObservableProperty]
    private string? fullNameError;

    [ObservableProperty]
    private string? phoneError;

    [ObservableProperty]
    private string? emailError;

    [ObservableProperty]
    private string? passwordError;

    [ObservableProperty]
    private string? confirmPasswordError;

    [ObservableProperty]
    private bool isPasswordVisible;

    [ObservableProperty]
    private bool isConfirmPasswordVisible;

    partial void OnFullNameChanged(string value)
    {
        if (_fullNameTouched) RefreshFullNameError();
    }

    partial void OnPhoneChanged(string value)
    {
        if (_phoneTouched) RefreshPhoneError();
    }

    partial void OnEmailChanged(string value)
    {
        if (_emailTouched) RefreshEmailError();
    }

    partial void OnPasswordChanged(string value)
    {
        if (_passwordTouched) RefreshPasswordError();

        // The web app's cross-field validator re-runs on every change to the FormGroup, so editing
        // Password alone re-checks an already-touched ConfirmPassword for a mismatch too.
        if (_confirmPasswordTouched) RefreshConfirmPasswordError();
    }

    partial void OnConfirmPasswordChanged(string value)
    {
        if (_confirmPasswordTouched) RefreshConfirmPasswordError();
    }

    public void TouchFullName() { _fullNameTouched = true; RefreshFullNameError(); }
    public void TouchPhone() { _phoneTouched = true; RefreshPhoneError(); }
    public void TouchEmail() { _emailTouched = true; RefreshEmailError(); }
    public void TouchPassword() { _passwordTouched = true; RefreshPasswordError(); }
    public void TouchConfirmPassword() { _confirmPasswordTouched = true; RefreshConfirmPasswordError(); }

    /// <summary>Restores the screen to a blank, untouched state — see LoginViewModel.Reset for why
    /// this is needed even though Shell can reuse the page instance.</summary>
    public void Reset()
    {
        FullName = string.Empty;
        Phone = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        SelectedRoleId = RoleId.Customer;

        FullNameError = null;
        PhoneError = null;
        EmailError = null;
        PasswordError = null;
        ConfirmPasswordError = null;
        ErrorMessage = null;
        IsPasswordVisible = false;
        IsConfirmPasswordVisible = false;

        _fullNameTouched = false;
        _phoneTouched = false;
        _emailTouched = false;
        _passwordTouched = false;
        _confirmPasswordTouched = false;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitAsync(CancellationToken cancellationToken)
    {
        TouchFullName();
        TouchPhone();
        TouchEmail();
        TouchPassword();
        TouchConfirmPassword();

        if (!IsFormValid())
        {
            return;
        }

        ErrorMessage = null;
        IsBusy = true;
        SubmitCommand.NotifyCanExecuteChanged();

        try
        {
            var trimmedEmail = Email.Trim();
            var request = new RegisterRequestDto
            {
                FullName = FullName,
                Phone = Phone,
                Email = trimmedEmail.Length > 0 ? trimmedEmail : null,
                Password = Password,
                RoleId = (byte)SelectedRoleId,
            };

            var user = await _authService.RegisterAsync(request, cancellationToken);
            await _navigationService.NavigateToRoleHomeAsync(user.RoleName);
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
            IsBusy = false;
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private async Task GoToLoginAsync() => await _navigationService.GoBackAsync();

    [RelayCommand]
    private void ToggleShowPassword() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private void ToggleShowConfirmPassword() => IsConfirmPasswordVisible = !IsConfirmPasswordVisible;

    private bool CanSubmit() => !IsBusy;

    /// <summary>Called right after Touch* has refreshed every *Error property, so this just reads
    /// the results rather than recomputing them.</summary>
    private bool IsFormValid() =>
        FullNameError is null && PhoneError is null && EmailError is null &&
        PasswordError is null && ConfirmPasswordError is null;

    private string? RefreshFullNameError() => FullNameError = FullName switch
    {
        var v when string.IsNullOrWhiteSpace(v) => "Enter your full name.",
        var v when v.Length < 2 => "Full name must be at least 2 characters.",
        var v when v.Length > 100 => "Full name must be at most 100 characters.",
        _ => null,
    };

    private string? RefreshPhoneError() => PhoneError = Phone switch
    {
        var v when string.IsNullOrWhiteSpace(v) => "Enter your phone number.",
        var v when !PhoneValidation.IsValid(v) => "Enter a valid phone number (7-15 digits).",
        var v when v.Length > 15 => "Phone number is too long.",
        _ => null,
    };

    /// <summary>Email is optional — an empty value is valid, matching the web app applying no
    /// Validators.required to this control.</summary>
    private string? RefreshEmailError() => EmailError = Email switch
    {
        var v when string.IsNullOrEmpty(v) => null,
        var v when !EmailValidation.IsValid(v) => "Enter a valid email address.",
        var v when v.Length > 150 => "Email is too long.",
        _ => null,
    };

    private string? RefreshPasswordError() => PasswordError = Password switch
    {
        var v when string.IsNullOrEmpty(v) => "Enter a password.",
        var v when v.Length < 8 => "Password must be at least 8 characters.",
        var v when v.Length > 100 => "Password must be at most 100 characters.",
        _ => null,
    };

    private string? RefreshConfirmPasswordError() => ConfirmPasswordError = ConfirmPassword switch
    {
        var v when string.IsNullOrEmpty(v) => "Confirm your password.",
        var v when v != Password => "Passwords do not match.",
        _ => null,
    };
}
