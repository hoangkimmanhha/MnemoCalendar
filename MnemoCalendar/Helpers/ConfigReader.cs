using System.IO;
using System.Text.Json.Nodes;

namespace MnemoCalendar.Helpers;

public static class ConfigReader
{
    /// <summary>
    /// Read the .config file and return specified data across nested keys.
    /// For example: ConfigReader.Parse("default", "tag", "color");
    /// </summary>
    public static string? Parse(params string[] keys)
    {
        if (keys is null || keys.Length == 0)
            return null;

        string filePath = PathHelper.BaseDirectory("mnemo.config.json");
        if (!File.Exists(filePath))
            return null;

        string jsonString = File.ReadAllText(filePath);
        JsonNode? current = JsonNode.Parse(jsonString);

        foreach (string key in keys)
        {
            if (current is null)
                return null;

            current = current[key];
        }

        return current is JsonValue val ? val.ToString() : null;
    }
}