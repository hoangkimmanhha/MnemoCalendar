using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing.Text;

namespace MnemoCalendar.ViewModels.Settings;

public partial class ThemeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name;
    public ThemeViewModel()
    {
        Name = "Hades";
    }
}