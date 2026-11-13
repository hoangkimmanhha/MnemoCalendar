using System.IO;
using System.Windows;
using MnemoCalendar.Data;

namespace MnemoCalendar;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DatabaseInitializer.Initialize();

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}