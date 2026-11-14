using System.IO;
using System.Text.Json;
using MnemoCalendar.Helpers;
using MnemoCalendar.Data;
using MnemoCalendar.Models;
using MnemoCalendar.Repositories;
using MnemoCalendar.Services;

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

        var calendarService = new CalendarService(db: "database.test");
        var tagRepository = new SqliteTagRepository(db: "database.test");

        // Test add multiple events and their tags.
        foreach (var calendarEv in sampleEvents)
        {
            long? createdId = calendarService.CreateEventTag(calendarEv);
            Console.WriteLine($"Added event [{createdId}]: {calendarEv.Title} with tags: {string.Join(", ", calendarEv.Tags.Select(x => x.Name))}");
        }

        // Show information of all available tags.
        var allTags = tagRepository.GetAllTags()!;

        Console.WriteLine("Available tags:");
        foreach (var tag in allTags)
        {
            Console.WriteLine($"Tag [{tag.Id}]: {tag.Name}\tColor:{tag.ColorHex}");
        }

        // Test edit title/description/location/tag (once at a time).
        Console.Write("Edit ID? ");
        var editId = Convert.ToInt32(Console.ReadLine());

        var toEdit = calendarService.GetEventTagById(editId);
        if (toEdit != null)
        {
            // Show event information.
            Console.WriteLine(toEdit.Info());

            // Edit table.
            Console.WriteLine("\nWhat to edit? (0~3)\n(0) Title\n(1) Description\n(2) Location\n(3) Tags");
            var fieldToEdit = Convert.ToInt32(Console.ReadLine());

            Console.Write("New value (split by comma if edit Tags)? ");
            var newValue = Console.ReadLine() ?? string.Empty;

            switch (fieldToEdit)
            {
                case 0:
                    toEdit.Title = newValue;
                    break;
                case 1:
                    toEdit.Description = newValue;
                    break;
                case 2:
                    toEdit.Location = newValue;
                    break;
                case 3:
                    toEdit.Tags = newValue
                        .Split(',')
                        .Select(x => new Tag { Name = x })
                        .ToList();
                    break;
            }

            calendarService.UpdateEventTag(toEdit);

            var updatedEvent = calendarService.GetEventTagById(editId);
            Console.WriteLine("NEW:");
            Console.WriteLine(updatedEvent?.Info());
        }
        else Console.WriteLine("Event ID not found!");

    }
}