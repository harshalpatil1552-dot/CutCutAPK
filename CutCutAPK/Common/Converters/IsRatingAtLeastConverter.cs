using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>True when the bound int rating is &gt;= the ConverterParameter threshold — drives which
/// of the five star buttons on the review form render filled, mirroring the web app's
/// [class.is-filled]="star &lt;= reviewRating()" check.</summary>
public sealed class IsRatingAtLeastConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int rating || parameter is not string parameterString || !int.TryParse(parameterString, out var threshold))
        {
            return false;
        }

        return rating >= threshold;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
