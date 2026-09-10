using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Mirrors the web app's `{{ salon.avgRating || 'No ratings yet' }}` — a zero average
/// (no reviews yet) reads as "No ratings yet" instead of "⭐ 0".</summary>
public sealed class RatingToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is decimal rating && rating > 0 ? $"⭐ {rating.ToString(CultureInfo.InvariantCulture)}" : "No ratings yet";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
