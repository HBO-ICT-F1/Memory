using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
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
        private readonly List<Card> _cards = new List<Card>();
        private readonly Dictionary<int, Image> cardImages = new Dictionary<int, Image>();
        private readonly double CardScaleHeight = 2;
        private readonly double CardScaleWidth = 2;
        private readonly Uri defaultCardImage;
        private readonly Grid grid = new Grid();
        private readonly TextBlock player1_text = new TextBlock();
        private readonly TextBlock player2_text = new TextBlock();
        private readonly List<int> selectedCards = new List<int>();
        private readonly Dictionary<int, int> shownCards = new Dictionary<int, int>();

        public bool gridLines = false;

        public bool multiplayer = false;
        private bool player1 = true;
        private int player1_score;

        private bool player2;
        private int player2_score;

        public GamePage()
        {
            InitializeComponent();
            var theme = App.GetInstance().theme;
            var images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{theme}/cards",
                    "*");
            defaultCardImage =
                new Uri(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{theme}/default.jpg");

            _cards = Card.Generate(images);
        }

        /// <summary>
        ///     Used to start games and score generation.
        /// </summary>
        public void Start()
        {
            GenerateScore();
            ShowCards();
        }

        /// <summary>
        ///     Generate Score grid.
        /// </summary>
        private void GenerateScore()
        {
            var scoreGrid = new Grid();
            scoreGrid.HorizontalAlignment = HorizontalAlignment.Left;
            scoreGrid.VerticalAlignment = VerticalAlignment.Top;
            scoreGrid.ShowGridLines = gridLines;

            var colDef1 = new ColumnDefinition();
            scoreGrid.ColumnDefinitions.Add(colDef1);

            var rowDef1 = new RowDefinition();
            var rowDef2 = new RowDefinition();
            scoreGrid.RowDefinitions.Add(rowDef1);
            scoreGrid.RowDefinitions.Add(rowDef2);

            player1_text.FontSize = 20;
            player1_text.FontWeight = FontWeights.Bold;
            Grid.SetColumn(player1_text, 0);
            Grid.SetRow(player1_text, 0);

            player2_text.FontSize = 20;
            player2_text.FontWeight = FontWeights.Bold;
            Grid.SetColumn(player2_text, 0);
            Grid.SetRow(player2_text, 1);

            scoreGrid.Children.Add(player1_text);
            scoreGrid.Children.Add(player2_text);
            CardBox.Children.Add(scoreGrid);
            UpdateCurrentPlayer();
        }

        /// <summary>
        ///     Update player score and player turn.
        /// </summary>
        private void UpdateCurrentPlayer()
        {
            if (player1)
            {
                player1_text.Foreground = new SolidColorBrush(Colors.Red);
                player2_text.Foreground = new SolidColorBrush(Colors.Black);
            }

            if (player2)
            {
                player1_text.Foreground = new SolidColorBrush(Colors.Black);
                player2_text.Foreground = new SolidColorBrush(Colors.Red);
            }

            player1_text.Text = $"Player 1: {player1_score}";

            var type = multiplayer ? "Player 2" : "Computer";
            player2_text.Text = $"{type}: {player2_score}";
        }

        /// <summary>
        ///     Generates playing area.
        /// </summary>
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
            grid.ShowGridLines = gridLines;

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
                    new BitmapImage(defaultCardImage);
                cardImages.Add(index, image);
                Grid.SetRow(image, y);
                Grid.SetColumn(image, x);

                grid.Children.Add(image);
                index++;
            }

            CardBox.Children.Add(grid);
        }

        /// <summary>
        ///     Button handler to detect card selected.
        /// </summary>
        private async void ButtonHandler(Card card, Image cardImage, int index)
        {
            if (selectedCards.Count >= 1 && selectedCards[0] == index || selectedCards.Count == 2) return;
            cardImage.Source = card.image;
            selectedCards.Add(index);

            if (!shownCards.ContainsKey(index)) shownCards.Add(index, card.type);

            if (selectedCards.Count < 2) return;
            await CheckCards();
            selectedCards.Clear();
        }

        /// <summary>
        ///     Check if selected cards are the same.
        /// </summary>
        private async Task CheckCards()
        {
            if (_cards[selectedCards[0]].type == _cards[selectedCards[1]].type)
            {
                if (player1) player1_score++;
                if (player2) player2_score++;

                await Task.Delay(500);
                grid.Children.Remove(cardImages[selectedCards[0]]);
                cardImages.Remove(selectedCards[0]);

                grid.Children.Remove(cardImages[selectedCards[1]]);
                cardImages.Remove(selectedCards[1]);

                shownCards.Remove(selectedCards[0]);
                shownCards.Remove(selectedCards[1]);

                SystemSounds.Asterisk.Play();
                UpdateCurrentPlayer();
                return;
            }

            await Task.Delay(1000);
            cardImages[selectedCards[0]].Source = cardImages[selectedCards[1]].Source =
                new BitmapImage(defaultCardImage);

            player1 = !player1;
            player2 = !player2;

            SystemSounds.Hand.Play();
            UpdateCurrentPlayer();
            if (!multiplayer) await ComputerAgent();
        }

        /// <summary>
        ///     Start turn of the computer agent.
        /// </summary>
        /// <returns>A task</returns>
        private async Task ComputerAgent()
        {
            var typeCount = new Dictionary<int, List<int>>();
            foreach (var card in shownCards)
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

            int? typeIndex = null;
            foreach (var type in typeCount)
                if (type.Value.Count == 2)
                {
                    typeIndex = type.Key;
                    break;
                }

            int cardOne;
            int cardTwo;
            if (typeIndex == null)
            {
                cardOne = await PickRandomKey();
                cardTwo = await PickRandomKey(cardOne);

                if (!shownCards.ContainsKey(cardOne)) shownCards.Add(cardOne, _cards[cardOne].type);

                if (!shownCards.ContainsKey(cardTwo)) shownCards.Add(cardTwo, _cards[cardTwo].type);

                await Task.Delay(600);
                cardImages[cardOne].Source = _cards[cardOne].image;
                await Task.Delay(600);
                cardImages[cardTwo].Source = _cards[cardTwo].image;
                await Task.Delay(500);
                if (_cards[cardOne].type != _cards[cardTwo].type)
                {
                    cardImages[cardOne].Source = cardImages[cardTwo].Source = new BitmapImage(defaultCardImage);
                    player1 = !player1;
                    player2 = !player2;
                    SystemSounds.Hand.Play();
                    UpdateCurrentPlayer();
                    return;
                }
            }
            else
            {
                cardOne = typeCount[(int) typeIndex][0];
                cardTwo = typeCount[(int) typeIndex][1];
                await Task.Delay(600);
                cardImages[typeCount[(int) typeIndex][0]].Source = _cards[typeCount[(int) typeIndex][0]].image;
                await Task.Delay(600);
                cardImages[typeCount[(int) typeIndex][1]].Source = _cards[typeCount[(int) typeIndex][1]].image;
                await Task.Delay(500);
            }

            if (shownCards.ContainsKey(cardOne)) shownCards.Remove(cardOne);

            if (shownCards.ContainsKey(cardTwo)) shownCards.Remove(cardTwo);

            grid.Children.Remove(cardImages[cardOne]);
            cardImages.Remove(cardOne);

            grid.Children.Remove(cardImages[cardTwo]);
            cardImages.Remove(cardTwo);

            shownCards.Remove(cardOne);
            shownCards.Remove(cardTwo);

            SystemSounds.Asterisk.Play();
            player2_score++;
            UpdateCurrentPlayer();

            if (grid.Children.Count > 0) await ComputerAgent();
        }

        /// <summary>
        ///     Picks a random key (card) for computer agent.
        /// </summary>
        private async Task<int> PickRandomKey(int? retryKey = null)
        {
            var rand = new Random();
            var keyList = new List<int>(cardImages.Keys);
            var key = keyList[rand.Next(keyList.Count)];
            if (key == retryKey)
            {
                await Task.Delay(10);
                return await PickRandomKey(retryKey);
            }

            return key;
        }
    }
}