using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for PreGame.xaml
    /// </summary>
    public partial class PreGame : Page
    {
        private int GameSize = 4;

        public PreGame()
        {
            InitializeComponent();
            Four.IsChecked = true;
        }

        private void FourByFour(object sender, RoutedEventArgs e)
        {
            GameSize = 4;
            Six.IsChecked = false;
            Eight.IsChecked = false;
        }

        private void SixbySix(object sender, RoutedEventArgs e)
        {
            GameSize = 6;
            Four.IsChecked = false;
            Eight.IsChecked = false;
        }

        private void EightByEight(object sender, RoutedEventArgs e)
        {
            GameSize = 8;
            Four.IsChecked = false;
            Six.IsChecked = false;
        }

        private void BattleRobot(object sender, RoutedEventArgs e)
        {
            var gamePage = new GamePage();
            // gamePage.multiplayer = false;
            gamePage.Start();
            MainWindow.GetMainWindow().ChangePage(gamePage);
        }
    }
}