using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Three-way label for the check-in-code button — mirrors the web app's
/// `{{ isFetchingOtp() ? 'Generating…' : (otpCode() ? 'Regenerate code' : 'Get check-in code') }}`.
/// Bind as a MultiBinding with [0] = IsFetchingOtp (bool), [1] = OtpCode (string?).</summary>
public sealed class OtpButtonTextConverter : IMultiValueConverter
{
    public object Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        var isFetching = values.Length > 0 && values[0] is true;
        var hasCode = values.Length > 1 && values[1] is string { Length: > 0 };

        if (isFetching)
        {
            return "Generating…";
        }

        return hasCode ? "Regenerate code" : "Get check-in code";
    }

    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
