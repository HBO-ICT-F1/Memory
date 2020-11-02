using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Memory.ui.pages
{
    /// <summary>
    ///     This initialize the Components
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly App _app;
        private readonly MainWindow _mainWindow;

        /// <summary>
        ///     This initialize the Components
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
            _app = App.GetInstance();
            _mainWindow = MainWindow.GetMainWindow();
            SetHandlerAndButtonDisable();
        }

        /// <summary>
        ///     Set all button handlers and disable current theme button
        /// </summary>
        private void SetHandlerAndButtonDisable()
        {
            switch (_app.Theme)
            {
                case "default":
                    Default.Background = Brushes.Gray;
                    Default.Click -= ThemeSpace;
                    break;
                case "dogs":
                    Dogs.Background = Brushes.Gray;
                    Dogs.Click -= ThemeDogs;
                    break;
            }

            Volume.Value = _app.Player.Volume;
        }

        /// <summary>
        ///     Change theme to the dogs theme
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void ThemeDogs(object sender, RoutedEventArgs routedEventArgs)
        {
            _app.Theme = "dogs";
            _mainWindow.ChangeTheme(new MainPage());
        }

        /// <summary>
        ///     Change theme to the default theme
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void ThemeSpace(object sender, RoutedEventArgs routedEventArgs)
        {
            _app.Theme = "default";
            _mainWindow.ChangeTheme(new MainPage());
        }

        /// <summary>
        ///     Change the music value
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments to get selected volume</param>
        private void ChangeVolume(object sender, RoutedPropertyChangedEventArgs<double> routedEventArgs)
        {
            _app.Player.Volume = routedEventArgs.NewValue;
        }

        /// <summary>
        ///     Navigate back to the previous page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="routedEventArgs">The route event arguments</param>
        private void Back(object sender, RoutedEventArgs routedEventArgs)
        {
            NavigationService.GetNavigationService(this)?.GoBack();
        }
    }
}