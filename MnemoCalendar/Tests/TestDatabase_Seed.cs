using System.IO;
using System.Text.Json;
using MnemoCalendar.Helpers;
using MnemoCalendar.Data;
using MnemoCalendar.Models;
using MnemoCalendar.Repositories;

namespace MnemoCalendar.Tests;

public class TestDatabase_Seed
{
    public void Test(string jsonFilePath = "test_events_1.json")
    {
        jsonFilePath = PathHelper.BaseDirectory("Tests", jsonFilePath);

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"File not found at: {jsonFilePath}");
        }

        string jsonContent = File.ReadAllText(jsonFilePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        List<CalendarEvent>? sampleEvents = JsonSerializer.Deserialize<List<CalendarEvent>>(jsonContent, options);

        if (sampleEvents is null || sampleEvents.Count == 0)
        {
            return;
        }

        var eventRepository = new SqliteEventRepository(db: "test.database");

        foreach (var calendarEv in sampleEvents)
        {
            long createdId = eventRepository.CreateEvent(calendarEv);
            Console.WriteLine($"Added event [{createdId}]: {calendarEv.Title}");
        }
    }
}