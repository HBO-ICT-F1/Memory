using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        private readonly List<Card> cards = new List<Card>();
        public Dictionary<int, int> shownCards = new Dictionary<int, int>();
        public Dictionary<int, Image> cardImages = new Dictionary<int, Image>();
        public double cardScaleHeight = 2;
        public double cardScaleWidth = 2;
        public Grid grid = new Grid();
        public List<int> selectedCards = new List<int>();
        public Uri defaultCardImage;

        public GamePage()
        {
            InitializeComponent();

            var images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/cards",
                    "*");
            defaultCardImage =
                new Uri(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/default/default.jpg");

            cards = Card.Generate(images);
            ShowCards();
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
            for (var y = 0; y < rows; y++)
            {
                var image = new Image();
                image.RenderSize = new Size(cardWidth, cardHeight);
                image.Stretch = Stretch.Fill;

                var currentIndex = index;
                var card = cards[index];
                image.MouseDown += (sender, e) =>
                {
                    var cardImage = sender as Image;
                    ButtonHandler(card, cardImage, currentIndex);
                };

                image.Source =
                    new BitmapImage(defaultCardImage);
                cardImages.Add(index, image);
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
            cardImage.Source = card.Image;
            selectedCards.Add(index);

            if (!shownCards.ContainsKey(index))
            {
                shownCards.Add(index, card.Type);
            }

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

                shownCards.Remove(selectedCards[0]);
                shownCards.Remove(selectedCards[1]);
                return;
            }

            await Task.Delay(1000);
            cardImages[selectedCards[0]].Source = cardImages[selectedCards[1]].Source =
                new BitmapImage(defaultCardImage);
            await ComputerAgent();
        }

        private async Task ComputerAgent()
        {
            var typeCount = new Dictionary<int, List<int>>();
            foreach (var card in shownCards)
            {
                if (!typeCount.ContainsKey(card.Value))
                {
                    var cards = new List<int>();
                    cards.Add(card.Key);
                    typeCount.Add(card.Value, cards);
                }
                else if (!typeCount[card.Value].Contains(card.Key))
                {
                    typeCount[card.Value].Add(card.Key);
                }
            }

            int? typeIndex = null;
            foreach (var type in typeCount)
            {
                if (type.Value.Count == 2)
                {
                    typeIndex = type.Key;
                    break;
                }
            }

            int cardOne;
            int cardTwo;
            if (typeIndex == null)
            {
                cardOne = await PickRandomKey();
                cardTwo = await PickRandomKey(cardOne);

                if (!shownCards.ContainsKey(cardOne))
                {
                    shownCards.Add(cardOne, cards[cardOne].Type);
                }

                if (!shownCards.ContainsKey(cardTwo))
                {
                    shownCards.Add(cardTwo, cards[cardTwo].Type);
                }

                await Task.Delay(1000);
                cardImages[cardOne].Source = cards[cardOne].Image;
                await Task.Delay(1000);
                cardImages[cardTwo].Source = cards[cardTwo].Image;
                await Task.Delay(1000);
                if (cards[cardOne].Type != cards[cardTwo].Type)
                {
                    cardImages[cardOne].Source = cardImages[cardTwo].Source =
                        new BitmapImage(defaultCardImage);
                    return;
                }
            }
            else
            {
                cardOne = typeCount[(int) typeIndex][0];
                cardTwo = typeCount[(int) typeIndex][1];
                await Task.Delay(1000);
                cardImages[typeCount[(int) typeIndex][0]].Source = cards[typeCount[(int) typeIndex][0]].Image;
                await Task.Delay(1000);
                cardImages[typeCount[(int) typeIndex][1]].Source = cards[typeCount[(int) typeIndex][1]].Image;
                await Task.Delay(500);
            }

            // TODO: increment score

            if (shownCards.ContainsKey(cardOne))
            {
                shownCards.Remove(cardOne);
            }

            if (shownCards.ContainsKey(cardTwo))
            {
                shownCards.Remove(cardTwo);
            }

            grid.Children.Remove(cardImages[cardOne]);
            cardImages.Remove(cardOne);

            grid.Children.Remove(cardImages[cardTwo]);
            cardImages.Remove(cardTwo);

            shownCards.Remove(cardOne);
            shownCards.Remove(cardTwo);
            if (grid.Children.Count > 0)
            {
                await ComputerAgent();
            }
        }

        private async Task<int> PickRandomKey(int? retryKey = null)
        {
            Random rand = new Random();
            var keyList = new List<int>(cardImages.Keys);
            int key = keyList[rand.Next(keyList.Count)];
            if (key == retryKey)
            {
                await Task.Delay(10);
                return await PickRandomKey(retryKey);
            }

            return key;
        }
    }
}