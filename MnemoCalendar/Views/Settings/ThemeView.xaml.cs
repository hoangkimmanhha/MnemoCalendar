using MnemoCalendar.ViewModels.Settings;

namespace MnemoCalendar.Views.Settings
{
    public partial class ThemeView
    {
        public ThemeView()
        {
            InitializeComponent();
            DataContext = new ThemeViewModel();
        }
    }
}
