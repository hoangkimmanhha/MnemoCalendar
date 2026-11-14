using MnemoCalendar.Models;

namespace MnemoCalendar.Repositories;

public interface IEventRepository
{
    /// <summary>
    /// Add a new event (include tags if any).
    /// </summary>
    /// <returns>long event_id</returns>
    public long CreateEvent(CalendarEvent calendarEvent);

    /// <summary>
    /// Get details of an event by its id.
    /// </summary>
    public CalendarEvent? GetEventById(long id); 

    /// <summary>
    /// Get all events in a given period of time.
    /// </summary>
    public List<CalendarEvent>? GetEventsInRange(DateTimeOffset fromUtc, DateTimeOffset toExclusiveUtc);

    /// <summary>
    /// Search and filter events that match the query by title (or any field).
    /// </summary>
    public List<CalendarEvent>? SearchEvents(string query);

    /// <summary>
    /// Permanently delete an event (and its related records).
    /// </summary>
    public void DeleteEvent(long id);

    /// <summary>
    /// Update details of an event (except for it Tag list). Overwrite information to exist CalendarEvent.Id.
    /// </summary>
    /// <param name="id"></param>
    public void UpdateEvent(CalendarEvent newCalendarEvent);
}