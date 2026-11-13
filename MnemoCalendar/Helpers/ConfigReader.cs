using System.IO;
using System.Text.Json.Nodes;

namespace MnemoCalendar.Helpers;
public static class ConfigReader
{    
    public static string? Parse(string toParse){
        string jsonString = File.ReadAllText(PathHelper.BaseDirectory("mnemo.config.json"));
        JsonNode? root = JsonNode.Parse(jsonString);

        if (root != null)
        {
            string? result = root[toParse]?.GetValue<string>();
            return result;
        }
        return null;
    }
}