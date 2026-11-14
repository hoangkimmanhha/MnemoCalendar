using Microsoft.Data.Sqlite;
using MnemoCalendar.Helpers;
using System.IO;

namespace MnemoCalendar.Data;

public class ConnectionStringBuilder
{
    /// <returns>Connection string to database with standard parameters (e.g. PRAGMA FK)</returns>
    public static string GetConnectionString(string fullpath)
    {
        string dataSource = fullpath;
        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = dataSource,
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true
        }.ToString();
        return connectionString;
    }
}