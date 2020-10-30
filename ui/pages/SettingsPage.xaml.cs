using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Memory.ui.pages
{
    public partial class SettingsPage : Page
    {
        private readonly App _app;
        private readonly MainWindow _mainWindow;

        public SettingsPage()
        {
            _app = App.GetInstance();
            _mainWindow = MainWindow.GetMainWindow();
            InitializeComponent();
            switch (_app.Theme)
            {
                case "default":
                    Default.Background = Brushes.Gray;
                    break;
                case "dogs":
                    Dogs.Background = Brushes.Gray;
                    break;
            }

            Volume.Value = _app.Player.Volume;
        }

        private void ThemeDogs(object sender, RoutedEventArgs e)
        {
            _app.Theme = "dogs";
            _mainWindow.ChangeTheme(new MainPage());
        }

        private void ThemeSpace(object sender, RoutedEventArgs e)
        {
            _app.Theme = "default";
            _mainWindow.ChangeTheme(new MainPage());
        }

        private void ChangeVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _app.Player.Volume = e.NewValue;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            var nav = NavigationService.GetNavigationService(this);
            nav.GoBack();
        }
    }
}