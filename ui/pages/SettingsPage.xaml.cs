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