namespace MnemoCalendar.Models;

public class CalendarEvent
{
    /// <summary>
    /// Leave blank if create new Tag (autoincrement).
    /// </summary>
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public List<Tag> Tags { get; set; } = new();
    public string? ColorHex { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
}