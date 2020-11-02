using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        /// <summary>
        ///     This initialize the Components
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Change page to PreGame Page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Play(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().ChangePage(new PreGame());
        }

        /// <summary>
        ///     Change page to Settings Page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Settings(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().settingsPage);
        }

        /// <summary>
        ///     Change page to Scoreboard page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Scoreboard(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.GetMainWindow().ChangePage(new ScoreboardPage());
        }

        /// <summary>
        ///     Quit application
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Quit(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow.QuitApplication();
        }
    }
}