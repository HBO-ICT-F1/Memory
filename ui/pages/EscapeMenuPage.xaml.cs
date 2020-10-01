using System.Windows;
using System.Windows.Controls;

namespace Memory.ui.pages
{
    public partial class EscapeMenuPage : Page
    {
        public EscapeMenuPage()
        {
            InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs routedEventArgs)
        {
            var main = MainWindow.GetMainWindow();
            main.escapeMenuToggle = !main.escapeMenuToggle;
            main.DrawEscapeMenu();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            MainWindow.QuitApplication();
        }
    }
}