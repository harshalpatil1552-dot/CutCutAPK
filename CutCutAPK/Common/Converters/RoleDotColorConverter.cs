using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>
/// Colors the role-card selection dot (see AuthStyles.xaml's RoleOptionRadio ControlTemplate)
/// straight off the RadioButton's own IsChecked via TemplateBinding. A VisualState/TargetName
/// Setter approach hit a XamlC compiler bug in this project (XC0001, "Cannot resolve property
/// Stroke on type ControlTemplate"), so this sidesteps VisualStateManager entirely.
/// ConverterParameter "Stroke" or "Fill" picks which of the two colors to return. Values are
/// duplicated from Colors.xaml's AuthGold / BrandBorder — keep them in sync if those change.
/// </summary>
public sealed class RoleDotColorConverter : IValueConverter
{
    private static readonly Color Gold = Color.FromArgb("#C98A2E");
    private static readonly Color BorderGray = Color.FromArgb("#E5E7EB");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
        {
            return Gold;
        }

        var isFill = string.Equals(parameter as string, "Fill", StringComparison.OrdinalIgnoreCase);
        return isFill ? Colors.Transparent : BorderGray;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
