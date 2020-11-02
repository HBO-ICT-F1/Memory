using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainPage.xaml
    /// </summary>
    public partial class SavedGamePage : Page
    {
        public SavedGamePage()
        {
            var window = MainWindow.GetMainWindow();
            InitializeComponent();
            var index = 1;
            App.GetInstance().Database.Query("SELECT * FROM `saves` ORDER BY `id` DESC LIMIT 10;", reader =>
            {
                while (reader.Read())
                {
                    index++;
                    var id = Convert.ToInt32(reader["id"]);
                    var btn = new Button();
                    btn.Background = Brushes.Orange;
                    btn.FontSize = 20;
                    btn.Content = "Game " + id;
                    btn.Height = 50;
                    btn.Width = 400;
                    btn.Margin = new Thickness(0, 500 - 100 * index, 0, 0);
                    btn.Click += (sender, args) =>
                    {
                        window.GamePage = new GamePage();
                        window.GamePage.Start(id);
                        window.ChangePage(window.GamePage);
                    };
                    Grid.Children.Add(btn);
                }
            });
        }
    }
}