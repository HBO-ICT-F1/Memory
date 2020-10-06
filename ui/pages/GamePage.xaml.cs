using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading.Tasks;
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
        public Grid grid = new Grid();
        public List<int> selectedCards = new List<int>();
        public Dictionary<int, Image> cardImages = new Dictionary<int, Image>();
        private List<Card> cards = new List<Card>();
        private MediaPlayer player;

        public GamePage()
        {
            MainWindow.GetMainWindow().activePage = this;
            InitializeComponent();
            var images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/cards",
                    "*");
            cards = Card.Generate(images);
            ShowCards();

            player = new MediaPlayer();
            player.Open(new Uri(
                ($"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/default.mp3"
                )));
            player.Play();
        }

        private void ShowCards()
        {
            var rows = Math.Sqrt(cards.Count);
            var columns = Math.Sqrt(cards.Count);
            var maxScale = Math.Min(SystemParameters.PrimaryScreenHeight / rows,
                SystemParameters.PrimaryScreenWidth / columns);
            var maxScaleSize = Math.Max(cardScaleHeight, cardScaleWidth);
            var cardHeight = (int) (cardScaleHeight * maxScale / maxScaleSize);
            var cardWidth = (int) (cardScaleWidth * maxScale / maxScaleSize);

            grid.Width = cardWidth * columns;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

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
                    var image = new Image();
                    image.RenderSize = new Size(cardWidth, cardHeight);
                    image.Stretch = Stretch.Fill;
                    var currentIndex = index;
                    var card = cards[index];
                    image.MouseDown += new MouseButtonEventHandler((sender, e) =>
                    {
                        var cardImage = sender as Image;
                        ButtonHandler(card, cardImage, currentIndex);
                    });
                    image.Source =
                        new BitmapImage(new Uri(
                            $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/default.jpg"));
                    cardImages.Add(index, image);
                    Grid.SetRow(image, y);
                    Grid.SetColumn(image, x);
                    grid.Children.Add(image);
                    index++;
                }
            }

            CardBox.Children.Add(grid);
        }

        private async void ButtonHandler(Card card, Image cardImage, int index)
        {
            if (selectedCards.Count >= 1 && selectedCards[0] == index || selectedCards.Count == 2) return;
            cardImage.Source = card.Image;
            selectedCards.Add(index);
            if (selectedCards.Count < 2) return;
            await CheckCards();
            selectedCards.Clear();
        }

        private async Task CheckCards()
        {
            if (cards[selectedCards[0]].Type == cards[selectedCards[1]].Type)
            {
                // TODO: increment score
                await Task.Delay(500);
                grid.Children.Remove(cardImages[selectedCards[0]]);
                cardImages.Remove(selectedCards[0]);
                grid.Children.Remove(cardImages[selectedCards[1]]);
                cardImages.Remove(selectedCards[1]);
                return;
            }

            await Task.Delay(1000);
            cardImages[selectedCards[0]].Source = cardImages[selectedCards[1]].Source =
                new BitmapImage(new Uri(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/default.jpg"));
        }
    }
}