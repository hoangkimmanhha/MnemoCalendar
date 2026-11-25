using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Data;

namespace MnemoCalendar.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly ThemeViewModel _themeVM = new();

    [ObservableProperty]
    private ObservableObject _currentSectionViewModel;

    public SettingsViewModel()
    {
        CurrentSectionViewModel = _themeVM;
    }
    [RelayCommand]
    public void Navigate(string section)
    {
        CurrentSectionViewModel = section switch
        {
            "Theme" => _themeVM,
            _ => _themeVM
        };
    }
}