using MnemoCalendar.Helpers;
using System;

public static class ColorHelper
{
    public static string? ValidateColorHex(string? hexColor)
    {
        if (string.IsNullOrWhiteSpace(hexColor)) return null;
        var str = hexColor.Trim();
        if (str.Length != 7 || str[0] != '#') return null;

        return str.Skip(1).All(char.IsAsciiHexDigit) ? str : null;
    }
    /// <summary>
    /// Return the hex code of given color name. If the color name is already hex code, return it.
    /// </summary>
    public static string? ColorHexCode(string? colorName)
    {
        return ValidateColorHex(colorName) ?? ConfigReader.Parse($"default.colors.{colorName}");
    }
}