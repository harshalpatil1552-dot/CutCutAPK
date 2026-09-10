using System.Text.RegularExpressions;

namespace CutCutAPK.Common.Validation;

/// <summary>
/// Practical email format check — a simpler equivalent of the web app's Validators.email regex,
/// not a byte-for-byte port of it (the two ecosystems' built-in email patterns already differ, and
/// the server's own [EmailAddress] validation on RegisterRequestDto.cs is the real source of truth
/// either way).
/// </summary>
public static partial class EmailValidation
{
    public static bool IsValid(string value) => !string.IsNullOrEmpty(value) && Pattern().IsMatch(value);

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex Pattern();
}
