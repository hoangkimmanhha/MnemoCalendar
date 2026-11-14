using Microsoft.Data.Sqlite;
using MnemoCalendar.Data;
using MnemoCalendar.Helpers;
using MnemoCalendar.Models;
using MnemoCalendar.Repositories;
using System;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Markup;

namespace MnemoCalendar.Services;

/// <summary>
/// Provide functions related to Event and Tag at the same time. Doesn't replace SqliteTagRepository.cs and SqliteEventRepository.cs
/// </summary>
public class CalendarService
{
    private readonly string _connectionString;
    private readonly string _db;
    public CalendarService(string db = "database.main")
    {
        _db = db;
        _connectionString = ConnectionStringBuilder.GetConnectionString(PathHelper.LocalAppData(ConfigReader.Parse(_db)!));
    }
    /// <summary>
    /// Create event and create its relationship with tag (create tag if doesn't exist).
    /// </summary>
    /// <returns>If there's no errors, return true, otherwise false.</returns>
    public long? CreateEventTag(CalendarEvent calendarEvent)
    {
        // Create event.
        var eventRepository = new SqliteEventRepository(_db);
        var tagRepository = new SqliteTagRepository(_db);

        long EventId = eventRepository.CreateEvent(calendarEvent);
        var TagId = new List<long>();
        
        // Create/Add tags.
        foreach (var tag in calendarEvent.Tags)
        {
            TagId.Add(tagRepository.CreateTag(tag));
        }

        // Update reference table.
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        using var transaction = connection.BeginTransaction();

        try
        {
            command.Transaction = transaction;
            command.CommandText =
                @"INSERT INTO event_tags (event_id, tag_id)
                VALUES ($event_id, $tag_id);";

            foreach (var tagId in TagId)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("$event_id", EventId);
                command.Parameters.AddWithValue("$tag_id", tagId);

                command.ExecuteNonQuery();
            }

            transaction.Commit();
            return EventId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    /// <summary>
    /// Get a CalendarEvent along with its associated Tags by event ID.
    /// </summary>
    public CalendarEvent? GetEventTagByID(long id)
    {
        var eventRepository = new SqliteEventRepository(_db);
        var tagRepository = new SqliteTagRepository(_db);

        var calendarEvent = eventRepository.GetEventById(id);
        if (calendarEvent is null)
            return null;

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT tag_id FROM event_tags WHERE event_id = $event_id;";
        command.Parameters.AddWithValue("$event_id", id);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            long tagId = reader.GetInt32(0);

            var tag = tagRepository.GetTagByID(tagId);
            if (tag is not null)
            {
                calendarEvent.Tags.Add(tag);
            }
        }

        return calendarEvent;
    }

    /// <summary>
    /// Modify event tag list of an exist event.
    /// </summary>
    /// <returns>True if updated sucessfully, otherwise false.</returns>
    public bool UpdateEventTag(CalendarEvent newCalendarEvent)
    {
        var eventRepository = new SqliteEventRepository(_db);
        var tagRepository = new SqliteTagRepository(_db);

        eventRepository.UpdateEvent(newCalendarEvent); // Update everything of event except for tag list.
        var newTagIds = new List<long>();


        // Create tag if there's new tag. 
        foreach (var tag in newCalendarEvent.Tags)
        {
            long id = tagRepository.CreateTag(tag);
            tag.Id = (int) id; // Sync Id for object (optional)
            newTagIds.Add(id);
        }

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {

            // Select all tag_id that related to newCalendarEvent.Id (database version).
            using var commandSelect = connection.CreateCommand();
            commandSelect.Transaction = transaction;
            commandSelect.CommandText =
                @"SELECT tag_id FROM event_tags
                WHERE event_id = $event_id";
            commandSelect.Parameters.AddWithValue("$event_id", newCalendarEvent.Id);

            var oldTagIds = new List<long>();
            using (var reader = commandSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    oldTagIds.Add(reader.GetInt64(0));
                }
            }

            // If a tag exist in old version, and does not exist in new version
            // -> That means user want to remove that tag
            // If a tag exist in new version, and does not exist in old version
            // -> Above function created/added that tag to both event_tags and tags table.
            var tagIdsToDelete = oldTagIds.Except(newTagIds).ToList();
            foreach (var tagId in tagIdsToDelete)
            {
                using var commandDelete = connection.CreateCommand();
                commandDelete.Transaction = transaction;
                commandDelete.CommandText =
                    @"DELETE FROM event_tags WHERE event_id = $event_id AND tag_id = $tag_id;";
                commandDelete.Parameters.AddWithValue("$event_id", newCalendarEvent.Id);
                commandDelete.Parameters.AddWithValue("$tag_id", tagId);
                commandDelete.ExecuteNonQuery();
            }

            // Add relationship to event_tags table (ignore if exist).
            foreach (var tagId in newTagIds)
            {
                using var commandAdd = connection.CreateCommand();
                commandAdd.Transaction = transaction;
                commandAdd.CommandText =
                    @"INSERT OR IGNORE INTO event_tags (event_id, tag_id)
                    VALUES ($event_id, $tag_id);";
                commandAdd.Parameters.AddWithValue("$event_id", newCalendarEvent.Id);
                commandAdd.Parameters.AddWithValue("$tag_id", tagId);

                commandAdd.ExecuteNonQuery();
            }
            transaction.Commit();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            transaction.Rollback();
            throw;
        }
    }
}
