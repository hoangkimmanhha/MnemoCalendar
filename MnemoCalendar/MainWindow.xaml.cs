using MnemoCalendar.ViewModels;
using System.Windows;

namespace MnemoCalendar
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}