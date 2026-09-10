using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Mirrors login.html's `{{ isSubmitting() ? 'Signing in…' : 'Sign in' }}` exactly.</summary>
public sealed class BusyToSignInTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? "Signing in…" : "Sign in";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
