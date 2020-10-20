using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Memory.card;

namespace Memory.ui.pages
{
    /// <summary>
    ///     Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public const double CardScaleHeight = 2;
        public const double CardScaleWidth = 2;
        private readonly List<Card> _cards;
        public Grid grid = new Grid();
        public Dictionary<int, Image> images = new Dictionary<int, Image>();
        public List<int> selectedCards = new List<int>();

        public GamePage()
        {
            InitializeComponent();

            var images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/cards",
                    "*");

            _cards = Card.Generate(images);
            ShowCards();
        }

        private void ShowCards()
        {
            var rows = Math.Sqrt(_cards.Count);
            var columns = Math.Sqrt(_cards.Count);

            var maxScale = Math.Min(SystemParameters.PrimaryScreenHeight / rows,
                SystemParameters.PrimaryScreenWidth / columns);
            var maxScaleSize = Math.Max(CardScaleHeight, CardScaleWidth);

            var cardHeight = (int) (CardScaleHeight * maxScale / maxScaleSize);
            var cardWidth = (int) (CardScaleWidth * maxScale / maxScaleSize);

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
            for (var y = 0; y < rows; y++)
            {
                var image = new Image();
                image.RenderSize = new Size(cardWidth, cardHeight);
                image.Stretch = Stretch.Fill;

                var currentIndex = index;
                var card = _cards[index];
                image.MouseDown += (sender, e) =>
                {
                    var cardImage = sender as Image;
                    ButtonHandler(card, cardImage, currentIndex);
                };

                image.Source =
                    new BitmapImage(new Uri(
                        $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/default.jpg"));
                images.Add(index, image);
                Grid.SetRow(image, y);
                Grid.SetColumn(image, x);

                grid.Children.Add(image);
                index++;
            }

            CardBox.Children.Add(grid);
        }

        private async void ButtonHandler(Card card, Image cardImage, int index)
        {
            if (selectedCards.Count >= 1 && selectedCards[0] == index || selectedCards.Count == 2) return;
            cardImage.Source = card.image;
            selectedCards.Add(index);

            if (selectedCards.Count < 2) return;
            await CheckCards();
            selectedCards.Clear();
        }

        private async Task CheckCards()
        {
            if (_cards[selectedCards[0]].type == _cards[selectedCards[1]].type)
            {
                // TODO: increment score
                await Task.Delay(500);
                grid.Children.Remove(images[selectedCards[0]]);
                images.Remove(selectedCards[0]);

                grid.Children.Remove(images[selectedCards[1]]);
                images.Remove(selectedCards[1]);
                return;
            }

            await Task.Delay(1000);
            images[selectedCards[0]].Source = images[selectedCards[1]].Source =
                new BitmapImage(new Uri(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/default.jpg"));
        }
    }
}