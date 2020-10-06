using System.Diagnostics;
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
            MainWindow.GetMainWindow().ChangePage(new GamePage());
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(new SettingsPage());
        }

        private void Scoreboard(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(new ScoreboardPage());
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            MainWindow.QuitApplication();
        }
    }
}