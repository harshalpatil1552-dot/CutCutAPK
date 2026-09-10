using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>Multi-value: true unless the bound "busy id" (an int?) equals this row's own id (an
/// int) — drives a per-row button's IsEnabled the way the web app's
/// `[disabled]="busyServiceId() === item.serviceId"` (and the same pattern for staff/bookings)
/// does. Bind as a MultiBinding with [0] = the page-level busy id, [1] = this row's id.</summary>
public sealed class IsNotBusyIdConverter : IMultiValueConverter
{
    public object Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Length < 2 || values[1] is not int rowId)
        {
            return true;
        }

        return values[0] is not int busyId || busyId != rowId;
    }

    public object[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
