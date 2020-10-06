using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class EscapeMenuPage : Page
    {
        private readonly MainWindow MainWindow;

        public EscapeMenuPage()
        {
            MainWindow = MainWindow.GetMainWindow();
            InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs routedEventArgs) => CloseMenu();

        private void Settings(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.ChangePage(MainWindow.settingsPage);
            CloseMenu();
        }

        private void Menu(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.ChangePage(MainWindow.mainPage);
            CloseMenu();
        }

        private void Quit(object sender, RoutedEventArgs e) => MainWindow.QuitApplication();

        private void CloseMenu()
        {
            MainWindow.escapeMenuToggle = false;
            MainWindow.DrawEscapeMenu();
        }
    }
}