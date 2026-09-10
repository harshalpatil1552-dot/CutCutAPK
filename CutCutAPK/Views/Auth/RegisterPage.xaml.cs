using CutCutAPK.ViewModels.Auth;

namespace CutCutAPK.Views.Auth;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Reset();
    }

    // Thin event-to-ViewModel forwarding only — see LoginPage for the same pattern.
    private void OnFullNameUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchFullName();
    private void OnPhoneUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchPhone();
    private void OnEmailUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchEmail();
    private void OnPasswordUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchPassword();
    private void OnConfirmPasswordUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchConfirmPassword();

    private void OnFullNameCompleted(object? sender, EventArgs e) => PhoneEntry.Focus();
    private void OnPhoneCompleted(object? sender, EventArgs e) => EmailEntry.Focus();
    private void OnEmailCompleted(object? sender, EventArgs e) => PasswordEntry.Focus();
    private void OnPasswordCompleted(object? sender, EventArgs e) => ConfirmPasswordEntry.Focus();

    private void OnConfirmPasswordCompleted(object? sender, EventArgs e)
    {
        if (_viewModel.SubmitCommand.CanExecute(null))
        {
            _viewModel.SubmitCommand.Execute(null);
        }
    }
}
