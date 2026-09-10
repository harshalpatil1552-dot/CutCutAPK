using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>True when the bound reference is null — the inverse of <see cref="IsNotNullConverter"/>,
/// used to switch between two mutually-exclusive blocks the way the web app's @if/@else does.</summary>
public sealed class IsNullConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
