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
    /// <summary>
    /// Leave blank
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; set; }
    /// <summary>
    /// Leave blank
    /// </summary>
    public DateTimeOffset UpdatedAtUtc { get; set; }

    public string Info()
    {
        string tagsText = Tags is { Count: > 0 }
            ? string.Join(", ", Tags.Select(t => $"{t.Name} ({t.ColorHex ?? "default"})"))
            : "None";

        return $@"
Event: {Title}
Event ID: {Id}
Start At: {StartAt}
End At: {EndAt:yyyy-MM-dd HH:mm zzz}
Location: {Location ?? "None"}
Description: {Description ?? "None"}
Color: {ColorHex ?? "None"}
Tags: {tagsText}
Created At: {CreatedAtUtc.LocalDateTime}
Updated At: { UpdatedAtUtc.LocalDateTime}";
    }
}