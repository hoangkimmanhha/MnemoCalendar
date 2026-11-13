using System.IO;
using System.Windows;
using MnemoCalendar.Data;
using MnemoCalendar.Tests;

namespace MnemoCalendar;

public partial class App : Application
{
    private void TestInitialize()
    {
        DatabaseInitializer.Initialize(db: "test.database");
        var tester = new TestDatabase_Seed();
        tester.Test();
    }

    private void NormalInitialize()
    {
        DatabaseInitializer.Initialize();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        NormalInitialize();

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}