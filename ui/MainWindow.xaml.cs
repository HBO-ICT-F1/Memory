using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Memory.ui.pages;

namespace Memory.ui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow mainWindow;
        private readonly Button escapeMenuButton;
        private readonly Rectangle escapeMenuRectangle;
        private bool escapeMenuToggle;

        public MainWindow()
        {
            mainWindow = this;
            escapeMenuToggle = false;
            InitializeComponent();
            ChangePage(new MainPage());
            Height = SystemParameters.PrimaryScreenHeight;
            Width = SystemParameters.PrimaryScreenWidth;
            escapeMenuRectangle = new Rectangle();
            escapeMenuRectangle.Height = 400;
            escapeMenuRectangle.Width = 600;
            escapeMenuRectangle.Fill = Brushes.CornflowerBlue;
            escapeMenuButton = new Button {Content = "Quit"};
            escapeMenuButton.Width = 100;
            escapeMenuButton.Height = 33;
            escapeMenuButton.FontSize = 20;
            escapeMenuButton.Click += (sender, args) => QuitApplication();
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
            UiFrame.Content = page;
        }

        private void EscapeMenu(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) escapeMenuToggle = !escapeMenuToggle;
            DrawEscapeMenu();
        }

        private void DrawEscapeMenu()
        {
            if (!escapeMenuToggle)
            {
                mainGrid.Children.Remove(escapeMenuButton);
                mainGrid.Children.Remove(escapeMenuRectangle);
                return;
            }

            mainGrid.Children.Add(escapeMenuRectangle);
            mainGrid.Children.Add(escapeMenuButton);
        }
    }
}