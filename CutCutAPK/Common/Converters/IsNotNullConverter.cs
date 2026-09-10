using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>True when the bound value is non-null — drives IsVisible for optional fields (e.g. a
/// salon's DistanceKm, only populated on a "near me" search).</summary>
public sealed class IsNotNullConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is not null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
