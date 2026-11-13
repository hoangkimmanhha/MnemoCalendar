using System.IO;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace MnemoCalendar.Data;

public class DatabaseInitializer
{
    /// <summary>
    /// Create database if it doesn't exist
    /// </summary>
    public static void Initialize()
    {
        using var connection = new SqliteConnection(ConnectionStringBuilder.GetConnectionString(GetDBPath()));
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(GetSchemaPath());
        command.ExecuteNonQuery();
    }

    private static string GetSchemaPath()
    {
        string schemaPath = Path.Combine(AppContext.BaseDirectory, "Data", "schema.sql");
        if (!File.Exists(schemaPath))
        {
            throw new FileNotFoundException("Cannot found schema file at: ", schemaPath);
        }

        return schemaPath;
    }
    public static string GetDBPath()
    {
        // Database file will be saved and used at "%LOCALAPPDATA%/MnemoCalendar/mnemocal.db"
        var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbFolder = Path.Combine(appDataFolder, "MnemoCalendar");
        Directory.CreateDirectory(dbFolder); // Create folder if not exists

        return Path.Combine(dbFolder, "mnemoCld.db");
    }
}