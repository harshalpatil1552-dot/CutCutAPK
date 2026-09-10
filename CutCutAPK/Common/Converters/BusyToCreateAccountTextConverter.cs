using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Mirrors register.html's `{{ isSubmitting() ? 'Creating account…' : 'Create account' }}`.</summary>
public sealed class BusyToCreateAccountTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is true ? "Creating account…" : "Create account";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
