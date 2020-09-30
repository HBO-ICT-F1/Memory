using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Memory.ui.pages;

namespace Memory.ui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow mainWindow;

        public MainWindow()
        {
            mainWindow = this;
            InitializeComponent();
            ChangePage(new MainPage());
            Height = SystemParameters.PrimaryScreenHeight;
            Width = SystemParameters.PrimaryScreenWidth;
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

        private void EscapeMenu()
        {
            while (true)
            {
                if (Keyboard.IsKeyDown(Key.Escape)) Debug.WriteLine("Pressed");
                Thread.Sleep(10);
                Debug.WriteLine("Not pressed");
            }
        }
    }
}