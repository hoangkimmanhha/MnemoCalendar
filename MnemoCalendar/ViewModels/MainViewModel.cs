using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MnemoCalendar.Views;
using System.Windows;

namespace MnemoCalendar.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    void ShowSettingsWindow()
    {
        var settingsWindow = new SettingsWindow
        {
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        settingsWindow.ShowDialog();
    }
}