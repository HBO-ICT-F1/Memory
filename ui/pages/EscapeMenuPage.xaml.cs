using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class EscapeMenuPage : Page
    {
        private readonly MainWindow mainWindow;

        public EscapeMenuPage()
        {
            mainWindow = MainWindow.GetMainWindow();
            InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseMenu();
        }

        private void Settings(object sender, RoutedEventArgs routedEventArgs)
        {
            mainWindow.ChangePage(new MainPage());
            CloseMenu();
        }

        private void Menu(object sender, RoutedEventArgs routedEventArgs)
        {
            mainWindow.ChangePage(new MainPage());
            CloseMenu();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            MainWindow.QuitApplication();
        }

        private void CloseMenu()
        {
            mainWindow.escapeMenuToggle = false;
            mainWindow.DrawEscapeMenu();
        }
    }
}