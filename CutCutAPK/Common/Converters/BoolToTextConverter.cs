using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Swaps a button's label based on a bool (typically an IsBusy/IsSubmitting flag) — mirrors
/// the web app's repeated `{{ isSubmitting() ? 'Doing…' : 'Do it' }}` ternaries. Pass
/// "WhenFalse|WhenTrue" as the ConverterParameter, e.g. "Confirm slot|Booking…".</summary>
public sealed class BoolToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var options = (parameter as string)?.Split('|') ?? Array.Empty<string>();
        var whenFalse = options.Length > 0 ? options[0] : string.Empty;
        var whenTrue = options.Length > 1 ? options[1] : whenFalse;

        return value is true ? whenTrue : whenFalse;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
