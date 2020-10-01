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
            InitializeComponent();
            
            ShowCards();
        }

        private void ShowCards()
        {
            string[] images = Directory.GetFiles($"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default", "*");
            List<Card> cards = Card.Generate(images);

            double rows = Math.Sqrt(cards.Count);
            double columns = Math.Sqrt(cards.Count);
            double maxScale = Math.Min(SystemParameters.PrimaryScreenHeight / rows, SystemParameters.PrimaryScreenWidth / columns);
            double maxScaleSize = Math.Max(cardScaleHeight, cardScaleWidth);
            int cardHeight = (int)(cardScaleHeight * maxScale / maxScaleSize);
            int cardWidth = (int)(cardScaleWidth * maxScale / maxScaleSize);
            
            Grid Grid = new Grid();
            Grid.Width = cardWidth * columns;
            Grid.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.VerticalAlignment = VerticalAlignment.Center;

            for (int i = 0; i < columns; i++)
            {
                ColumnDefinition Column = new ColumnDefinition();
                Grid.ColumnDefinitions.Add(Column);
            }
            for (int i = 0; i < rows; i++)
            {
                RowDefinition Row = new RowDefinition();
                Row.Height = new GridLength(cardHeight);
                Grid.RowDefinitions.Add(Row);
                Grid.ShowGridLines = true;
            }

            int index = 0;
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Image image = new Image();
                    image.RenderSize = new Size(cardWidth, cardHeight);
                    image.Stretch = Stretch.Fill;
                    Card card = cards[index];
                    image.MouseDown += new MouseButtonEventHandler((sender, e) => {
                        Image cardImage = sender as Image;
                        ButtonHandler(card, cardImage);
                    });
                    image.Source = new BitmapImage(new Uri($"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default.jpg"));
                    Grid.SetRow(image, y);
                    Grid.SetColumn(image, x);
                    Grid.Children.Add(image);
                    index++;
                }
            }
            CardBox.Children.Add(Grid);
        }

        private void ButtonHandler(Card card, Image cardImage)
        {
            // sets.Add(button);
            Debug.WriteLine("test");
            cardImage.Source = card.Image;
        }
    }
}
