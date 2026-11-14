using Microsoft.Data.Sqlite;
using MnemoCalendar.Data;
using MnemoCalendar.Models;
using MnemoCalendar.Helpers;
using System.Globalization;

namespace MnemoCalendar.Repositories;

public class SqliteEventRepository: IEventRepository
{
    private readonly string _connectionString;
    
    public SqliteEventRepository(string db = "database.main")
    {
        _connectionString = ConnectionStringBuilder.GetConnectionString(PathHelper.LocalAppData(ConfigReader.Parse(db)!));
    }
    /// <summary>
    /// Create event by given CalendarEvent, ignore calendarEvent.Id (autoincrement).
    /// </summary>
    /// <returns>long Id</returns>
    public long CreateEvent(CalendarEvent calendarEvent)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"INSERT INTO events(title, description, location, start_at_utc, end_at_utc, color_hex, created_at_utc, updated_at_utc)
            VALUES ($title, $description, $location, $start_at_utc, $end_at_utc, $color_hex, $created_at_utc, $updated_at_utc);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$title", calendarEvent.Title);
        command.Parameters.AddWithValue("$description", calendarEvent.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$location", calendarEvent.Location ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$start_at_utc", calendarEvent.StartAt.UtcDateTime.ToString("O"));
        command.Parameters.AddWithValue("$end_at_utc", calendarEvent.EndAt.UtcDateTime.ToString("O"));
        command.Parameters.AddWithValue("$color_hex", calendarEvent.ColorHex ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$created_at_utc", DateTimeOffset.Now.UtcDateTime.ToString("O"));
        command.Parameters.AddWithValue("$updated_at_utc", DateTimeOffset.Now.UtcDateTime.ToString("O"));

        long id = Convert.ToInt64(command.ExecuteScalar());
        return id;
    }
    public CalendarEvent? GetEventById(long id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT id, title, description, location, start_at_utc, end_at_utc,
                    color_hex, created_at_utc, updated_at_utc 
            FROM events
            WHERE id=$id";
        command.Parameters.AddWithValue("$id", id);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new CalendarEvent
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) == false ? reader.GetString(2) : "",
                Location = reader.IsDBNull(3) == false ? reader.GetString(3) : "",

                StartAt = DateTimeOffset.Parse(reader.GetString(4),
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind).ToLocalTime(),

                EndAt = DateTimeOffset.Parse(reader.GetString(5),
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind).ToLocalTime(),

                ColorHex = reader.IsDBNull(6) == false ? reader.GetString(6) : "",

                CreatedAtUtc = DateTimeOffset.Parse(reader.GetString(7),
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind),

                UpdatedAtUtc = DateTimeOffset.Parse(reader.GetString(8),
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind)
            };
        } else return null;
    }
    public List<CalendarEvent>? GetEventsInRange(DateTimeOffset fromUtc, DateTimeOffset toExclusiveUtc)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT * FROM events
            WHERE start_at_utc>=$start_at_utc AND end_at_utc<$end_at_utc;";
        command.Parameters.AddWithValue("$start_at_utc", fromUtc.ToString("O"));
        command.Parameters.AddWithValue("$end_at_utc", toExclusiveUtc.ToString("O"));

        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var result = new List<CalendarEvent>();
            while (reader.Read())
            {
                var Id = reader.GetInt32(0);
                result.Add(GetEventById(Id)!);
            }
            return result;
        }
        else return null;
    }
    public List<CalendarEvent>? SearchEvents(string query)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"SELECT * FROM events
            WHERE title LIKE $query OR description LIKE $query OR location LIKE $query;";

        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var result = new List<CalendarEvent>();
            while (reader.Read())
            {
                var Id = reader.GetInt32(0);
                result.Add(GetEventById(Id)!);
            }
            return result;
        }
        else return null;
    }

    public void UpdateEvent(CalendarEvent newCalendarEvent)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"UPDATE events
            SET title = $title, 
                description = $description,
                location = $location, 
                start_at_utc = $start_at_utc,
                end_at_utc = $end_at_utc,
                color_hex = $color_hex,
                created_at_utc = $created_at_utc,
                updated_at_utc = $updated_at_utc
            WHERE id = $id;";
        command.Parameters.AddWithValue("$id", newCalendarEvent.Id);
        command.Parameters.AddWithValue("$title", newCalendarEvent.Title);
        command.Parameters.AddWithValue("$description", newCalendarEvent.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$location", newCalendarEvent.Location ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$start_at_utc", newCalendarEvent.StartAt.UtcDateTime.ToString("O"));
        command.Parameters.AddWithValue("$end_at_utc", newCalendarEvent.EndAt.UtcDateTime.ToString("O"));
        command.Parameters.AddWithValue("$color_hex", newCalendarEvent.ColorHex ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$created_at_utc", newCalendarEvent.CreatedAtUtc.ToString("O"));
        command.Parameters.AddWithValue("$updated_at_utc", DateTimeOffset.Now.UtcDateTime.ToString("O"));

        command.ExecuteNonQuery();
    }
    public void DeleteEvent(long id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"DELETE FROM events WHERE id=$id;";

        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
    }
}