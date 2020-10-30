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

        public static MainWindow GetMainWindow()
        {
            return mainWindow;
        }

        public static void QuitApplication()
        {
            Environment.Exit(0);
        }

        public void ChangePage(Page page)
        {
            _activePage = page;
            UiFrame.Content = page;
        }

        private void EscapeMenu(object sender, KeyEventArgs e)
        {
            if (_activePage.Title != "Game" || DateTime.Now.ToFileTime() <= _escapeMenuDelay) return;
            if (e.Key == Key.Escape) escapeMenuToggle = !escapeMenuToggle;

            DrawEscapeMenu();
            _escapeMenuDelay = DateTime.Now.ToFileTime() + 5000000; //0.5S
        }

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