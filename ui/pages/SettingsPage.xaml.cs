using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class SettingsPage : Page
    {
        private readonly MainWindow _mainWindow;

        public SettingsPage()
        {
            _mainWindow = MainWindow.GetMainWindow();
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

        private void ListBoxItem_Selected(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected_1(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Slider_OnValueChanged(object sender,
            RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            var volume = routedPropertyChangedEventArgs.NewValue;
            App.GetInstance().Player.Volume = volume;

            var db = App.GetInstance().Database;
            db.Query($@"INSERT OR REPLACE INTO `settings` VALUES('volume', '{volume}')");
        }
    }
}