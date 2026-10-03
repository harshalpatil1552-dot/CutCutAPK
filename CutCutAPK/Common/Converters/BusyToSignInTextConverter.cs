using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Based on login.html's `{{ isSubmitting() ? 'Signing in…' : 'Sign in' }}` — the trailing
/// arrow on the idle label is specific to the mobile app's redesigned login screen.</summary>
public sealed class BusyToSignInTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? "Signing in…" : "Sign In  →";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
