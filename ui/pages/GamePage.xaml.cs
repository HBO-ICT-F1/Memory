using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Memory.card;
using Path = System.IO.Path;

namespace Memory.ui.pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public double cardScaleHeight = 2;
        public double cardScaleWidth = 1.5;

        public GamePage()
        {
            MainWindow.GetMainWindow().activePage = this;
            InitializeComponent();
            ShowCards();
        }

        private void ShowCards()
        {
            string[] images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default",
                    "*");
            List<Card> cards = Card.Generate(images);

            var rows = Math.Sqrt(cards.Count);
            var columns = Math.Sqrt(cards.Count);
            var maxScale = Math.Min(SystemParameters.PrimaryScreenHeight / rows,
                SystemParameters.PrimaryScreenWidth / columns);
            var maxScaleSize = Math.Max(cardScaleHeight, cardScaleWidth);
            var cardHeight = (int) (cardScaleHeight * maxScale / maxScaleSize);
            var cardWidth = (int) (cardScaleWidth * maxScale / maxScaleSize);

            var grid = new Grid
            {
                Width = cardWidth * columns,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            for (var i = 0; i < columns; i++)
            {
                var column = new ColumnDefinition();
                grid.ColumnDefinitions.Add(column);
            }

            for (var i = 0; i < rows; i++)
            {
                var row = new RowDefinition();
                row.Height = new GridLength(cardHeight);
                grid.RowDefinitions.Add(row);
                grid.ShowGridLines = true;
            }

            var index = 0;
            for (var x = 0; x < columns; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    var image = new Image {RenderSize = new Size(cardWidth, cardHeight), Stretch = Stretch.Fill};
                    var card = cards[index];
                    image.MouseDown += new MouseButtonEventHandler((sender, e) =>
                    {
                        var cardImage = sender as Image;
                        ButtonHandler(card, cardImage);
                    });
                    image.Source =
                        new BitmapImage(new Uri(
                            $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default.jpg"));
                    Grid.SetRow(image, y);
                    Grid.SetColumn(image, x);
                    grid.Children.Add(image);
                    index++;
                }
            }

            CardBox.Children.Add(grid);
        }

        private void ButtonHandler(Card card, Image cardImage)
        {
            // sets.Add(button);
            Debug.WriteLine("test");
            cardImage.Source = card.Image;
        }
    }
}