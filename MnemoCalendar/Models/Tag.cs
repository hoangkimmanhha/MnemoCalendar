namespace MnemoCalendar.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Name_ascii { get; set; } = string.Empty;
    public string? Color_hex { get; set; }
}