namespace MnemoCalendar.Models;

public class Tag
{
    /// <summary>
    /// Leave blank if create new Tag (autoincrement).
    /// </summary>
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Leave blank (auto generate).
    /// </summary>
    public string NameAscii { get; set; } = string.Empty;
    /// <summary>
    /// Optional, default tag color at ./mnemo.config.json/default.tag.color
    /// </summary>
    public string? ColorHex { get; set; }

    public string Info()
    {
        return $@"
Tag: {Name}
ASCII: {NameAscii}
Color: {ColorHex}";
    }
}