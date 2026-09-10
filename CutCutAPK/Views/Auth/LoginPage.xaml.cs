using CutCutAPK.ViewModels.Auth;

namespace CutCutAPK.Views.Auth;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // A fresh, untouched form every time this screen is reached — covers both the very first
        // launch and returning here after a logout.
        _viewModel.Reset();
    }

    // Entry has no built-in "blur" concept to bind directly to a command, so these thin handlers
    // forward the platform event straight to the ViewModel — no logic lives here beyond that.
    private void OnPhoneOrEmailUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchPhoneOrEmail();

    private void OnPasswordUnfocused(object? sender, FocusEventArgs e) => _viewModel.TouchPassword();

    private void OnPhoneOrEmailCompleted(object? sender, EventArgs e) => PasswordEntry.Focus();

    private void OnPasswordCompleted(object? sender, EventArgs e)
    {
        if (_viewModel.SubmitCommand.CanExecute(null))
        {
            _viewModel.SubmitCommand.Execute(null);
        }
    }
}
