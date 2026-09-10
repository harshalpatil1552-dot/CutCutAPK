using System.Globalization;
using System.Linq;

namespace CutCutAPK.Common.Converters;

/// <summary>True when the bound enum's name matches (or is one of a comma-separated list in) the
/// ConverterParameter — used to gate status-specific UI blocks the way the web app's
/// `@if (b.bookingStatusId === BookingStatusId.X)` (or an X-or-Y check) does, e.g.
/// ConverterParameter="CheckedIn,InProgress".</summary>
public sealed class EnumEqualsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is not string parameterString)
        {
            return false;
        }

        var candidates = parameterString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var valueName = value.ToString();

        return candidates.Any(candidate => string.Equals(candidate, valueName, StringComparison.OrdinalIgnoreCase));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
