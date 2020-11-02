using System;
using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class EscapeMenuPage : Page
    {
        private readonly MainWindow _mainWindow;

        /// <summary>
        ///     This initialize the Components and MainWindow
        /// </summary>
        public EscapeMenuPage()
        {
            _mainWindow = MainWindow.GetMainWindow();
            InitializeComponent();
        }

        /// <summary>
        ///     Continue button handler
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Continue(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseMenu();
        }

        /// <summary>
        ///     Settings button handler
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Settings(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainWindow.ChangePage(_mainWindow.settingsPage);
            SaveGame();
            CloseMenu();
        }

        /// <summary>
        ///     Reset game button handler
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Reset(object sender, RoutedEventArgs routedEventArgs)
        {
            var gamePage = new GamePage();
            gamePage.Start(null, _mainWindow.GamePage.Multiplayer, (int) Math.Sqrt(_mainWindow.GamePage.GameSize),
                _mainWindow.GamePage.Player1Name, _mainWindow.GamePage.Player2Name);
            _mainWindow.GamePage = gamePage;
            _mainWindow.ChangePage(gamePage);
            CloseMenu();
        }

        /// <summary>
        ///     Main menu button handler
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Menu(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainWindow.ChangePage(_mainWindow.mainPage);
            SaveGame();
            CloseMenu();
        }

        /// <summary>
        ///     Quit application button handler
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Quit(object sender, RoutedEventArgs e)
        {
            MainWindow.QuitApplication();
        }

        /// <summary>
        ///     Close escape menu
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void CloseMenu()
        {
            _mainWindow.escapeMenuToggle = false;
            _mainWindow.DrawEscapeMenu();
        }

        /// <summary>
        ///     Save game handler
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void SaveGame()
        {
            MainWindow.GetMainWindow().GamePage.Save();
            MainWindow.GetMainWindow().GamePage = null;
        }
    }
}