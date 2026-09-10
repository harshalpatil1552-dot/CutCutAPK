using System.Text.RegularExpressions;

namespace CutCutAPK.Common.Validation;

/// <summary>
/// Loose phone-number check — digits with an optional leading '+', 7 to 15 digits total, matching
/// the Users.Phone column width (VARCHAR(15)) and the backend's permissive [Phone] validation.
/// Mirrors the web app's shared/validators/phone.validator.ts exactly (same pattern).
/// </summary>
public static partial class PhoneValidation
{
    public static bool IsValid(string value) => !string.IsNullOrEmpty(value) && Pattern().IsMatch(value);

    [GeneratedRegex(@"^\+?\d{7,15}$")]
    private static partial Regex Pattern();
}
