using MnemoCalendar.ViewModels.UserControls;
using System.Windows.Controls;

namespace MnemoCalendar.Views.UserControls
{
    public partial class Clock : UserControl
    {
        public Clock()
        {
            InitializeComponent();
            DataContext = new ClockViewModel();
        }
    }
}
