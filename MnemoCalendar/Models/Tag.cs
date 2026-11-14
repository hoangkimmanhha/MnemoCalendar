namespace MnemoCalendar.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameAscii { get; set; } = string.Empty;
    public string? ColorHex { get; set; }
}