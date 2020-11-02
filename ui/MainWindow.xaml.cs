using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Memory.ui.pages;
using Path = System.IO.Path;

namespace Memory.ui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow mainWindow;
        private readonly Frame _escapeMenu;
        private readonly Rectangle _escapeMenuBg;

        public readonly MainPage mainPage;
        public readonly SettingsPage settingsPage;
        private Page _activePage;
        private long _escapeMenuDelay;
        public bool escapeMenuToggle;
        public GamePage GamePage;

        /// <summary>
        ///     This initialize the Components and configures some default variables
        /// </summary>
        public MainWindow()
        {
            mainWindow = this;
            escapeMenuToggle = false;
            _escapeMenuDelay = DateTime.Now.ToFileTime();
            mainPage = new MainPage();
            settingsPage = new SettingsPage();

            InitializeComponent();
            ChangePage(mainPage);

            Height = SystemParameters.PrimaryScreenHeight;
            Width = SystemParameters.PrimaryScreenWidth;

            _escapeMenu = new Frame {Content = new EscapeMenuPage(), Width = Width / 2, Height = Height / 2};
            _escapeMenuBg = new Rectangle
            {
                Height = Height, Width = Width, Fill = new SolidColorBrush(Color.FromArgb(150, 35, 35, 35))
            };

            var backGround = new ImageBrush();
            var image = new Image
            {
                Source = new BitmapImage(new Uri(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{App.GetInstance().Theme}/background.jpg")
                )
            };
            backGround.ImageSource = image.Source;
            mainWindow.Background = backGround;
        }

        /// <summary>
        ///     Get the main window object
        /// </summary>
        public static MainWindow GetMainWindow()
        {
            return mainWindow;
        }

        /// <summary>
        ///     Quit the application
        /// </summary>
        public static void QuitApplication()
        {
            Environment.Exit(0);
        }

        /// <summary>
        ///     Change the current page
        /// </summary>
        /// <param name="page">A object of the instance Page is used to change to a page</param>
        public void ChangePage(Page page)
        {
            if (page.Title != "Settings") _activePage = page;
            UiFrame.Content = page;
        }

        /// <summary>
        ///     Configure the theme that is previous configured
        /// </summary>
        /// <param name="page">A object of the instance Page is used to change to a page</param>
        public void ChangeTheme(Page page)
        {
            var app = App.GetInstance();
            var db = app.Database;
            // Save the selected theme
            db.Query($@"INSERT OR REPLACE INTO `settings` VALUES('theme', '{app.Theme}')");

            // Change theme music
            App.GetInstance().Player.Stop();
            App.GetInstance().Player.Open(new Uri(
                $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{App.GetInstance().Theme}/default.mp3"));
            App.GetInstance().Player.Play();

            var window = new MainWindow();
            window.Show();
            Close();
            ChangePage(page);
        }

        /// <summary>
        ///     Change the current page
        /// </summary>
        /// <param name="sender">A object of the button</param>
        /// <param name="keyEventArgs">The key event arguments</param>
        private void EscapeMenu(object sender, KeyEventArgs keyEventArgs)
        {
            if (_activePage.Title != "Game" || DateTime.Now.ToFileTime() <= _escapeMenuDelay) return;
            if (keyEventArgs.Key == Key.Escape) escapeMenuToggle = !escapeMenuToggle;

            DrawEscapeMenu();
            _escapeMenuDelay = DateTime.Now.ToFileTime() + 5000000; //0.5S
        }

        /// <summary>
        ///     Generate the scape menu page
        /// </summary>
        public void DrawEscapeMenu()
        {
            if (!escapeMenuToggle)
            {
                mainGrid.Children.Remove(_escapeMenuBg);
                mainGrid.Children.Remove(_escapeMenu);
                return;
            }

            mainGrid.Children.Add(_escapeMenuBg);
            mainGrid.Children.Add(_escapeMenu);
        }
    }
}