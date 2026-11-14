using System.IO;
using System.Text;
using System.Windows;
using MnemoCalendar.Data;
using MnemoCalendar.Helpers;
using MnemoCalendar.Tests;

namespace MnemoCalendar;

public partial class App : Application
{
    private void TestInitialize()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        DatabaseInitializer.Initialize(db: "database.test");
        var tester = new TestDatabase_Seed();
        tester.Test();
    }

    private void NormalInitialize()
    {
        DatabaseInitializer.Initialize(db: "database.main");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // NormalInitialize();
        TestInitialize();

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}