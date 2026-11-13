using System.IO;

namespace MnemoCalendar.Helpers;

public static class PathHelper
{
    public static string LocalAppData(params string[] paths)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MnemoCalendar", Path.Combine(paths));
    }
    public static string BaseDirectory(params string[] paths)
    {
        return Path.Combine(AppContext.BaseDirectory, Path.Combine(paths));
    }
}