using MnemoCalendar.ViewModels.UserControls;
using System.Windows.Controls;

namespace MnemoCalendar.Views.UserControls
{
    public partial class MiniClock : UserControl
    {
        public MiniClock()
        {
            InitializeComponent();
            DataContext = new ClockViewModel();
        }
    }
}
