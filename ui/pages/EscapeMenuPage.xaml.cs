using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class EscapeMenuPage : Page
    {
        private readonly MainWindow _mainWindow;

        public EscapeMenuPage()
        {
            _mainWindow = MainWindow.GetMainWindow();
            InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseMenu();
        }

        private void Settings(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainWindow.ChangePage(_mainWindow.settingsPage);
            SaveGame();
            CloseMenu();
        }

        private void Menu(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainWindow.ChangePage(_mainWindow.mainPage);
            SaveGame();
            CloseMenu();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            MainWindow.QuitApplication();
        }

        private void CloseMenu()
        {
            _mainWindow.escapeMenuToggle = false;
            _mainWindow.DrawEscapeMenu();
        }

        private void SaveGame()
        {
            MainWindow.GetMainWindow().GamePage.Save();
            MainWindow.GetMainWindow().GamePage = null;
        }
    }
}