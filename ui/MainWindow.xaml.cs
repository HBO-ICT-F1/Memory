using Memory.ui.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly MainWindow Window = new MainWindow();
        
        public MainWindow()
        {
           
            InitializeComponent();
            ChangePage(new MainPage());
            Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
        }

        public static void QuitApplication()
        {
            Environment.Exit(0);
        }

        public void ChangePage(Page page)
        {
            _UIFrame.Content = page;
        }
    }
}
