using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            var gamePage = new GamePage {Multiplayer = false};
            gamePage.Start();
            MainWindow.GetMainWindow().ChangePage(gamePage);
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().settingsPage);
        }

        private void Scoreboard(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().scoreboardPage);
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            MainWindow.QuitApplication();
        }
    }
}