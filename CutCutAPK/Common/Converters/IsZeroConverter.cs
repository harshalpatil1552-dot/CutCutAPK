using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>True when the bound int is zero — used to show a "nothing here yet" line driven off a
/// collection's Count, for lists too short to warrant the full EmptyState title+subtitle pair
/// (e.g. the web app's plain `@if (reviews().length === 0)` text on salon-detail).</summary>
public sealed class IsZeroConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is int count && count == 0;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
