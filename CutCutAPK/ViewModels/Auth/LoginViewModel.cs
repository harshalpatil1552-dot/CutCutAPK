using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CutCutAPK.Common.Exceptions;
using CutCutAPK.Navigation;
using CutCutAPK.Services.Auth;
using CutCutAPK.ViewModels.Base;

namespace CutCutAPK.ViewModels.Auth;

/// <summary>
/// Backs the login screen. Mirrors features/auth/login/login.ts field-for-field: same two
/// required fields, same "touch to reveal validation error" behavior (FieldErrorComponent), same
/// submit guard (disabled while in flight instead of allowing duplicate requests), and the same
/// role-based redirect on success.
/// </summary>
public sealed partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    private bool _phoneOrEmailTouched;
    private bool _passwordTouched;

    public LoginViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    private string phoneOrEmail = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string? phoneOrEmailError;

    [ObservableProperty]
    private string? passwordError;

    [ObservableProperty]
    private bool isPasswordVisible;

    partial void OnPhoneOrEmailChanged(string value)
    {
        if (_phoneOrEmailTouched)
        {
            RefreshPhoneOrEmailError();
        }
    }

    partial void OnPasswordChanged(string value)
    {
        if (_passwordTouched)
        {
            RefreshPasswordError();
        }
    }

    /// <summary>Called from the Entry's Unfocused handler — the code-behind equivalent of Angular
    /// marking a form control "touched" on blur, which is what turns validation errors on.</summary>
    public void TouchPhoneOrEmail()
    {
        _phoneOrEmailTouched = true;
        RefreshPhoneOrEmailError();
    }

    public void TouchPassword()
    {
        _passwordTouched = true;
        RefreshPasswordError();
    }

    /// <summary>Restores the screen to a blank, untouched state. Called when the page appears so a
    /// user who logs out and returns to login doesn't see the previous session's leftover input or
    /// error message — Angular gets this for free because loadComponent creates a fresh component
    /// instance per visit; Shell can reuse the same page instance, so we reset explicitly.</summary>
    public void Reset()
    {
        PhoneOrEmail = string.Empty;
        Password = string.Empty;
        PhoneOrEmailError = null;
        PasswordError = null;
        ErrorMessage = null;
        IsPasswordVisible = false;
        _phoneOrEmailTouched = false;
        _passwordTouched = false;
    }

    [RelayCommand]
    private void ToggleShowPassword() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitAsync(CancellationToken cancellationToken)
    {
        TouchPhoneOrEmail();
        TouchPassword();

        if (!IsFormValid())
        {
            return;
        }

        ErrorMessage = null;
        IsBusy = true;
        SubmitCommand.NotifyCanExecuteChanged();

        try
        {
            var user = await _authService.LoginAsync(PhoneOrEmail.Trim(), Password, cancellationToken);
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
    private async Task GoToRegisterAsync() => await _navigationService.NavigateToRegisterAsync();

    /// <summary>Prevents duplicate login requests: once a submit is in flight, the button is both
    /// visually disabled (bound to IsBusy) and the command itself refuses to execute again.</summary>
    private bool CanSubmit() => !IsBusy;

    private bool IsFormValid() => !string.IsNullOrWhiteSpace(PhoneOrEmail) && !string.IsNullOrWhiteSpace(Password);

    private void RefreshPhoneOrEmailError() =>
        PhoneOrEmailError = string.IsNullOrWhiteSpace(PhoneOrEmail) ? "Enter your phone number or email." : null;

    private void RefreshPasswordError() =>
        PasswordError = string.IsNullOrWhiteSpace(Password) ? "Enter your password." : null;
}
