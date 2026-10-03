using System.Globalization;

namespace CutCutAPK.Common.Converters;

/// <summary>
/// Colors a role card's Border in RegisterPage.xaml from its own RadioButton's IsChecked, read via
/// an x:Reference binding (see the DataTemplate) rather than a shared BindingContext — the
/// simplest reliable way found to react to "is this item selected" from inside a per-item
/// template. ConverterParameter "Stroke" or "Fill" picks which color to return. Values are
/// duplicated from Colors.xaml's AuthGold / BrandBorder / BrandSurface / AuthGoldLight — keep them
/// in sync if those change.
/// </summary>
public sealed class RoleCardColorConverter : IValueConverter
{
    private static readonly Color Gold = Color.FromArgb("#C98A2E");
    private static readonly Color GoldLight = Color.FromArgb("#FBF0DC");
    private static readonly Color BorderGray = Color.FromArgb("#E5E7EB");
    private static readonly Color White = Color.FromArgb("#FFFFFF");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isChecked = value is true;
        var isFill = string.Equals(parameter as string, "Fill", StringComparison.OrdinalIgnoreCase);

        if (isFill)
        {
            return isChecked ? GoldLight : White;
        }

        return isChecked ? Gold : BorderGray;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
