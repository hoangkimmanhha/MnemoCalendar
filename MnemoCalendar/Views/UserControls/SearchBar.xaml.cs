using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using System.Windows.Controls;

namespace MnemoCalendar.Views.UserControls
{
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        private void SearchBarInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBarInput.Text))
            {
                PlaceHolder.Visibility = Visibility.Visible;
            }
            else
            {
                PlaceHolder.Visibility = Visibility.Hidden;
            }
        }
    }
}
