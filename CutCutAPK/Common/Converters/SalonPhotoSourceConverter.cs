using System.Globalization;
using CutCutAPK.Common.Constants;
using CutCutAPK.Models.Salons;

namespace CutCutAPK.Common.Converters;

/// <summary>Turns a SalonResponseDto into an image source for the API's anonymous
/// GET salons/{id}/photo endpoint, or null when the salon has no photo (the cream placeholder
/// underneath then shows through).</summary>
public sealed class SalonPhotoSourceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SalonResponseDto { HasPhoto: true } salon)
        {
            return null;
        }

        return ImageSource.FromUri(new Uri($"{ApiConstants.BaseUrl}salons/{salon.SalonId}/photo"));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
