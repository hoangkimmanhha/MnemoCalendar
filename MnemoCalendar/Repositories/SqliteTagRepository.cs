using MnemoCalendar.Data;
using MnemoCalendar.Models;
using MnemoCalendar.Helpers;
using Microsoft.Data.Sqlite;

namespace MnemoCalendar.Repositories;

public class SqliteTagRepository: ITagRepository
{
    private readonly string _connectionString;

    public SqliteTagRepository(string db = "database")
    {
        _connectionString = ConnectionStringBuilder.GetConnectionString(PathHelper.LocalAppData(ConfigReader.Parse(db)!));
    }
    public long CreateTag(Tag tag)
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @"INSERT INTO tags (name, name_ascii, color_hex)
            VALUES ($name,$name_ascii, $color_hex);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$name", tag.Name);
        command.Parameters.AddWithValue("$name_ascii", tag.NameAscii);
        command.Parameters.AddWithValue("$color_hex", tag.ColorHex ?? (object)DBNull.Value);

        long id = Convert.ToInt32(command.ExecuteScalar());
        // Unique check, color check
        return id;
    }

    public Tag? GetTagByID(long id)
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT * FROM tags
            WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Tag
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                NameAscii = reader.GetString(2),
                ColorHex = !reader.IsDBNull(3) ? reader.GetString(3) : null
            };
        }
        else return null;
    }

    public List<Tag>? GetAllTags()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"SELECT * FROM tags";

        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var result = new List<Tag>();
            while (reader.Read())
            {
                result.Add(new Tag
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    NameAscii = reader.GetString(2),
                    ColorHex = reader.GetString(3) ?? ""
                });
            }
            return result;
        }
        else
        {
            return null;
        }
    }

    public void DeleteTag(long id)
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText =
            @"DELETE FROM tags
            WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
    }
}