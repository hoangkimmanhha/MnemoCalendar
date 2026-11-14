using System.Globalization;
using System.Text;

namespace MnemoCalendar.Helpers;

public static class StringHelper
{
	/// <summary>
	/// Removes diacritics from the specified text.
	/// </summary>
	/// <returns>A new version of the text with diacritics removed</returns>
	public static string RemoveDiacritics(this string text)
	{
		if (string.IsNullOrEmpty(text))
			return text;

		string normalized = text.Normalize(NormalizationForm.FormD);
		var builder = new StringBuilder();

		foreach (char c in normalized)
		{
			if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				builder.Append(c);
		}

		return builder.ToString()
			.Replace('đ', 'd')
			.Replace('Đ', 'D')
			.Normalize(NormalizationForm.FormC);
	}
	public static bool ColorStringCheck(string hex_color)
	{
        var str = hex_color.Trim();
        if (str.Length != 7 || str[0] != '#') return false;

        return str.Skip(1).All(char.IsAsciiHexDigit);
    }
}