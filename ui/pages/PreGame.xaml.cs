using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for PreGame.xaml
    /// </summary>
    public partial class PreGame : Page
    {
        private int _gameSize = 4;

        public PreGame()
        {
            InitializeComponent();
            Four.IsChecked = true;
        }

        private void FourByFour(object sender, RoutedEventArgs e)
        {
            _gameSize = 4;
            Six.IsChecked = false;
            Eight.IsChecked = false;
        }

        private void SixbySix(object sender, RoutedEventArgs e)
        {
            _gameSize = 6;
            Four.IsChecked = false;
            Eight.IsChecked = false;
        }

        private void EightByEight(object sender, RoutedEventArgs e)
        {
            _gameSize = 8;
            Four.IsChecked = false;
            Six.IsChecked = false;
        }

        private void BattleRobot(object sender, RoutedEventArgs e)
        {
            var gamePage = new GamePage(false, _gameSize, PlayerOne.Text, PlayerTwo.Text);
            gamePage.Start();
            MainWindow.GetMainWindow().ChangePage(gamePage);
        }

        private void MultiPlayer(object sender, RoutedEventArgs e)
        {
            var gamePage = new GamePage(true, _gameSize, PlayerOne.Text, PlayerTwo.Text);
            gamePage.Start();
            MainWindow.GetMainWindow().ChangePage(gamePage);
        }

        private void Online(object sender, RoutedEventArgs e)
        {
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().ChangePage(new MainPage());
        }

        private void LastGame(object sender, RoutedEventArgs e)
        {
        }
    }
}