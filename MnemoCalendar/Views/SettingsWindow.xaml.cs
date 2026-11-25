using System.Windows;
using MnemoCalendar.ViewModels;

namespace MnemoCalendar.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }
    }
}
