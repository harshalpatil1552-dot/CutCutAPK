using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Formats a salon's optional DistanceKm ("X.X km away") — mirrors the web app's
/// `{{ salon.distanceKm | number: '1.1-1' }} km away`.</summary>
public sealed class DistanceKmToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is double distanceKm ? $"{distanceKm.ToString("0.0", CultureInfo.InvariantCulture)} km away" : string.Empty;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
