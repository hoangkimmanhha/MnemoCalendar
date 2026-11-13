using System.IO;
using System.Windows;
using Microsoft.Data.Sqlite;
using MnemoCalendar.Helpers;

namespace MnemoCalendar.Data;

public class DatabaseInitializer
{
    /// <summary>
    /// Create database if it doesn't exist
    /// </summary>
    public static void Initialize(string db = "database")
    {
        using var connection = new SqliteConnection(ConnectionStringBuilder.GetConnectionString(CreateDBFolder(db)));
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(GetSchemaPath());
        command.ExecuteNonQuery();
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
    private static string CreateDBFolder(string db = "database")
    {
        Directory.CreateDirectory(PathHelper.LocalAppData()); // Create folder if not exists

        return PathHelper.LocalAppData(ConfigReader.Parse(db)!);
    }
}