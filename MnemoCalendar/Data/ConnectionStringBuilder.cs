using Microsoft.Data.Sqlite;
using System.IO;

namespace MnemoCalendar.Data;

public class ConnectionStringBuilder
{
    /// <returns>Connection string to database with standard parameters (e.g. PRAGMA FK)</returns>
    public static string GetConnectionString(in string dataSource)
    {
        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = dataSource,
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true
        }.ToString();
        return connectionString;
    }
}