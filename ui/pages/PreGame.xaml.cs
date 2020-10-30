using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for PreGame.xaml
    /// </summary>
    public partial class PreGame : Page
    {
        private int _gameSize = 4;

        public PreGame()
        {
            InitializeComponent();
            Four.IsChecked = true;
        }

        private void FourByFour(object sender, RoutedEventArgs e)
        {
            _gameSize = 4;
            Six.IsChecked = false;
            Eight.IsChecked = false;
        }

        private void SixbySix(object sender, RoutedEventArgs e)
        {
            _gameSize = 6;
            Four.IsChecked = false;
            Eight.IsChecked = false;
        }

        private void EightByEight(object sender, RoutedEventArgs e)
        {
            _gameSize = 8;
            Four.IsChecked = false;
            Six.IsChecked = false;
        }

        private void BattleRobot(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().GamePage = new GamePage();
            MainWindow.GetMainWindow().GamePage.Start(false, _gameSize);
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().GamePage);
        }

        private void MultiPlayer(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindow().GamePage = new GamePage();
            MainWindow.GetMainWindow().GamePage.Start(true, _gameSize);
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().GamePage);
        }

        private void Online(object sender, RoutedEventArgs e)
        {
        }
    }
}