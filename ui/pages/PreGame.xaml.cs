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

        /// <summary>
        ///     This initialize the Components
        /// </summary>
        public PreGame()
        {
            InitializeComponent();
            Four.IsChecked = true;
        }

        /// <summary>
        ///     Change selected game size to 4x4
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void FourByFour(object sender, RoutedEventArgs routedEventArgs)
        {
            _gameSize = 4;
            Six.IsChecked = false;
            Eight.IsChecked = false;
        }

        /// <summary>
        ///     Change selected game size to 6x6
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void SixbySix(object sender, RoutedEventArgs routedEventArgs)
        {
            _gameSize = 6;
            Four.IsChecked = false;
            Eight.IsChecked = false;
        }

        /// <summary>
        ///     Change selected game size to 8x8
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void EightByEight(object sender, RoutedEventArgs routedEventArgs)
        {
            _gameSize = 8;
            Four.IsChecked = false;
            Six.IsChecked = false;
        }

        /// <summary>
        ///     Change page to Game Page and start the game against a computer
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void BattleRobot(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().GamePage = new GamePage();
            MainWindow.GetMainWindow().GamePage.Start(null, false, _gameSize, PlayerOne.Text, PlayerTwo.Text);
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().GamePage);
        }

        /// <summary>
        ///     Change page to Game Page and start the game
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void MultiPlayer(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().GamePage = new GamePage();
            MainWindow.GetMainWindow().GamePage.Start(null, true, _gameSize, PlayerOne.Text, PlayerTwo.Text);
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().GamePage);
        }

        /// <summary>
        ///     Start a online game
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Online(object sender, RoutedEventArgs routedEventArgs)
        {
            //TODO: Add online functionality with sockets
        }

        /// <summary>
        ///     Change page to Main Page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Back(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().ChangePage(new MainPage());
        }

        /// <summary>
        ///     Change page to SavedGame Page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void LastGame(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().ChangePage(new SavedGamePage());
        }
    }
}