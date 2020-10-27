using System;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().mainPage);



        }
    }
}