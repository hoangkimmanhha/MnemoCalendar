using System.IO;
using System.Text.Json.Nodes;

public static class ConfigReader
{    
    public static string? Parse(string toParse){
        string jsonString = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "mnemo.config.json"));
        JsonNode? root = JsonNode.Parse(jsonString);

        if (root != null)
        {
            string? result = root[toParse]?.GetValue<string>();
            return result;
        }
        return null;
    }
}