using System.IO;
using System.Windows;
using Microsoft.Data.Sqlite;
using MnemoCalendar.Helpers;
using MnemoCalendar.Repositories;
using MnemoCalendar.Models;

namespace MnemoCalendar.Data;

public class DatabaseInitializer
{
    /// <summary>
    /// Create database if it doesn't exist, then add default tags.
    /// </summary>
    public static void Initialize(string db = "database.main")
    {
        using var connection = new SqliteConnection(ConnectionStringBuilder.GetConnectionString(CreateDBFolder(db)));
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(GetSchemaPath());
        command.ExecuteNonQuery();

        // Add default tags.
        var sqliteTagRepository = new SqliteTagRepository(db);
        foreach (var (name, color) in ConfigReader.ParseDictionary("default.tag.items"))
        {
            sqliteTagRepository.CreateTag(new Tag
            {
                Name = name,
                ColorHex = color
            });
        }
    }

    private static string GetSchemaPath()
    {
        string schemaPath = PathHelper.BaseDirectory("Data", ConfigReader.Parse("schema")!);
        if (!File.Exists(schemaPath))
        {
            throw new FileNotFoundException("Cannot found schema file at: ", schemaPath);
        }

        return schemaPath;
    }
    /// <summary>
    /// Create database folder at "%LOCALAPPDATA%/MnemoCalendar/."
    /// </summary>
    /// <returns>string fullDatabasePath</returns>
    private static string CreateDBFolder(string db = "database.main")
    {
        Directory.CreateDirectory(PathHelper.LocalAppData()); // Create folder if not exists

        return PathHelper.LocalAppData(ConfigReader.Parse(db)!);
    }
}