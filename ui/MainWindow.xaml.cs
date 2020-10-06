﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private readonly Frame escapeMenu;
        private readonly Rectangle escapeMenuBg;
        private long escapeMenuDelay;
        public bool escapeMenuToggle;
        public Page activePage;

        public MainWindow()
        {
            mainWindow = this;
            escapeMenuToggle = false;
            escapeMenuDelay = DateTime.Now.ToFileTime();
            InitializeComponent();
            ChangePage(new MainPage());
            activePage = new MainPage();
            Height = SystemParameters.PrimaryScreenHeight;
            Width = SystemParameters.PrimaryScreenWidth;
            escapeMenu = new Frame {Content = new EscapeMenuPage(), Width = Width / 2, Height = Height / 2};
            escapeMenuBg = new Rectangle
            {
                Height = Height, Width = Width, Fill = new SolidColorBrush(Color.FromArgb(150, 35, 35, 35))
            };
            var backGround = new ImageBrush();
            var image = new Image
            {
                Source = new BitmapImage(new Uri("https://miro.medium.com/max/10514/1*TG8yT-bltiG0FcRpx3YkRA.jpeg"))
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
            UiFrame.Content = page;
        }

        private void EscapeMenu(object sender, KeyEventArgs e)
        {
            if (activePage.Title != "Game" && DateTime.Now.ToFileTime() <= escapeMenuDelay) return;
            if (e.Key == Key.Escape) escapeMenuToggle = !escapeMenuToggle;
            DrawEscapeMenu();
            escapeMenuDelay = DateTime.Now.ToFileTime() + 5000000; //0.5S
        }

        public void DrawEscapeMenu()
        {
            if (!escapeMenuToggle)
            {
                mainGrid.Children.Remove(escapeMenuBg);
                mainGrid.Children.Remove(escapeMenu);
                return;
            }

            mainGrid.Children.Add(escapeMenuBg);
            mainGrid.Children.Add(escapeMenu);
        }
    }
}