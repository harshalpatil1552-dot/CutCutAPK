using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>True when the bound string is non-null/non-empty — used to drive the visibility of
/// the field-error and server-error labels, the same way the web app's @if (serverError(); as
/// error) / FieldErrorComponent only render when there's a message.</summary>
public sealed class StringNullOrEmptyToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        !string.IsNullOrEmpty(value as string);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
