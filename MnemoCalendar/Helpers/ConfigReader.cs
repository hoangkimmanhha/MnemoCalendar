using System.IO;
using System.Text.Json.Nodes;

namespace MnemoCalendar.Helpers;

public static class ConfigReader
{
    /// <summary>
    /// Read the .config file and return specified data using dot-separated path (e.g. "data.default.color").
    /// Use for single value.
    /// </summary>
    public static string? Parse(string keyPath)
    {
        if (string.IsNullOrWhiteSpace(keyPath))
            return null;

        string filePath = PathHelper.BaseDirectory("mnemo.config.json");
        if (!File.Exists(filePath))
            return null;

        string jsonString = File.ReadAllText(filePath);
        JsonNode? root = JsonNode.Parse(jsonString);
        if (root is null)
            return null;

        JsonNode? current = root;
        foreach (string segment in keyPath.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            current = current?[segment];
            if (current is null)
                return null;
        }

        return current is JsonValue val ? val.ToString() : null;
    }
    /// <summary>
    /// Read the .config file and return specified data using dot-separated path (e.g. "data.default.color").
    /// Use for List values.
    /// </summary>
    public static List<string> ParseList(string keyPath)
    {
        if (string.IsNullOrWhiteSpace(keyPath))
            return new List<string>();

        string filePath = PathHelper.BaseDirectory("mnemo.config.json");
        if (!File.Exists(filePath))
            return new List<string>();

        JsonNode? current = JsonNode.Parse(File.ReadAllText(filePath));
        foreach (string segment in keyPath.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            current = current?[segment];
            if (current is null)
                return new List<string>();
        }

        if (current is JsonArray array)
        {
            return array
                .Select(item => item?.ToString())
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .ToList()!;
        }

        return new List<string>();
    }
}