using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly List<Card> _cards;
        private readonly Dictionary<int, Image> _cardImages = new Dictionary<int, Image>();
        private const double CardScaleHeight = 2;
        private const double CardScaleWidth = 2;
        private readonly Uri _defaultCardImage;
        private readonly Grid _grid = new Grid();
        private readonly List<int> _selectedCards = new List<int>();
        private readonly Dictionary<int, int> _shownCards = new Dictionary<int, int>();
        private const bool GridLines = false;

        private readonly bool _multiplayer;

        private bool _player1 = true;
        private int _player1Score;
        private readonly TextBlock _player1Text = new TextBlock();

        private bool _player2;
        private int _player2Score;
        private readonly TextBlock _player2Text = new TextBlock();

        public GamePage(bool multiplayer, int gameSize)
        {
            InitializeComponent();
            _multiplayer = multiplayer;
            
            var theme = MainWindow.GetMainWindow().theme;
            var images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{theme}/cards",
                    "*");
            images = images.ToList().Take((gameSize * gameSize) / 2).ToArray();
            _defaultCardImage =
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
            var scoreGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = GridLines
            };

            var colDef1 = new ColumnDefinition();
            scoreGrid.ColumnDefinitions.Add(colDef1);

            var rowDef1 = new RowDefinition();
            var rowDef2 = new RowDefinition();
            scoreGrid.RowDefinitions.Add(rowDef1);
            scoreGrid.RowDefinitions.Add(rowDef2);

            _player1Text.FontSize = 20;
            _player1Text.FontWeight = FontWeights.Bold;
            Grid.SetColumn(_player1Text, 0);
            Grid.SetRow(_player1Text, 0);

            _player2Text.FontSize = 20;
            _player2Text.FontWeight = FontWeights.Bold;
            Grid.SetColumn(_player2Text, 0);
            Grid.SetRow(_player2Text, 1);

            scoreGrid.Children.Add(_player1Text);
            scoreGrid.Children.Add(_player2Text);
            CardBox.Children.Add(scoreGrid);
            UpdateCurrentPlayer();
        }

        /// <summary>
        ///     Update player score and player turn.
        /// </summary>
        private void UpdateCurrentPlayer()
        {
            if (_player1)
            {
                _player1Text.Foreground = new SolidColorBrush(Colors.Red);
                _player2Text.Foreground = new SolidColorBrush(Colors.Black);
            }

            if (_player2)
            {
                _player1Text.Foreground = new SolidColorBrush(Colors.Black);
                _player2Text.Foreground = new SolidColorBrush(Colors.Red);
            }

            _player1Text.Text = $"Player 1: {_player1Score}";

            var type = _multiplayer ? "Player 2" : "Computer";
            _player2Text.Text = $"{type}: {_player2Score}";
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

            _grid.Width = cardWidth * columns;
            _grid.HorizontalAlignment = HorizontalAlignment.Center;
            _grid.VerticalAlignment = VerticalAlignment.Center;
            _grid.ShowGridLines = GridLines;

            for (var i = 0; i < columns; i++)
            {
                var column = new ColumnDefinition();
                _grid.ColumnDefinitions.Add(column);
            }

            for (var i = 0; i < rows; i++)
            {
                var row = new RowDefinition();
                row.Height = new GridLength(cardHeight);
                _grid.RowDefinitions.Add(row);
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
                    new BitmapImage(_defaultCardImage);
                _cardImages.Add(index, image);
                Grid.SetRow(image, y);
                Grid.SetColumn(image, x);

                _grid.Children.Add(image);
                index++;
            }

            CardBox.Children.Add(_grid);
        }

        /// <summary>
        ///     Button handler to detect card selected.
        /// </summary>
        private async void ButtonHandler(Card card, Image cardImage, int index)
        {
            if (_selectedCards.Count >= 1 && _selectedCards[0] == index || _selectedCards.Count == 2) return;
            cardImage.Source = card.image;
            _selectedCards.Add(index);

            if (!_shownCards.ContainsKey(index)) _shownCards.Add(index, card.type);

            if (_selectedCards.Count < 2) return;
            await CheckCards();
            _selectedCards.Clear();
            if (_grid.Children.Count <= 0) GameFinished();
        }

        /// <summary>
        ///     Quit game page when game finished.
        /// </summary>
        private void GameFinished()
        {
            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().mainPage);
        }

        /// <summary>
        ///     Check if selected cards are the same.
        /// </summary>
        private async Task CheckCards()
        {
            if (_cards[_selectedCards[0]].type == _cards[_selectedCards[1]].type)
            {
                if (_player1) _player1Score++;
                if (_player2) _player2Score++;

                await Task.Delay(500);
                _grid.Children.Remove(_cardImages[_selectedCards[0]]);
                _cardImages.Remove(_selectedCards[0]);

                _grid.Children.Remove(_cardImages[_selectedCards[1]]);
                _cardImages.Remove(_selectedCards[1]);

                _shownCards.Remove(_selectedCards[0]);
                _shownCards.Remove(_selectedCards[1]);

                SystemSounds.Asterisk.Play();
                UpdateCurrentPlayer();
                return;
            }

            await Task.Delay(1000);
            _cardImages[_selectedCards[0]].Source = _cardImages[_selectedCards[1]].Source =
                new BitmapImage(_defaultCardImage);

            _player1 = !_player1;
            _player2 = !_player2;

            SystemSounds.Hand.Play();
            UpdateCurrentPlayer();
            if (!_multiplayer) await ComputerAgent();
        }

        /// <summary>
        ///     Start turn of the computer agent.
        /// </summary>
        /// <returns>A task</returns>
        private async Task ComputerAgent()
        {
            var typeCount = new Dictionary<int, List<int>>();
            
            // 0 1 2 3 
            //
            // 
            //
            //
            
            foreach (var card in _shownCards)
                if (!typeCount.ContainsKey(card.Value))
                {
                    var cards = new List<int> {card.Key};
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
                cardOne = PickRandomKey(typeCount);
                cardTwo = PickRandomKey(typeCount, cardOne);

                if (!_shownCards.ContainsKey(cardOne)) _shownCards.Add(cardOne, _cards[cardOne].type);

                if (!_shownCards.ContainsKey(cardTwo)) _shownCards.Add(cardTwo, _cards[cardTwo].type);

                await Task.Delay(600);
                _cardImages[cardOne].Source = _cards[cardOne].image;
                await Task.Delay(600);
                _cardImages[cardTwo].Source = _cards[cardTwo].image;
                await Task.Delay(500);
                if (_cards[cardOne].type != _cards[cardTwo].type)
                {
                    _cardImages[cardOne].Source = _cardImages[cardTwo].Source = new BitmapImage(_defaultCardImage);
                    _player1 = !_player1;
                    _player2 = !_player2;
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
                _cardImages[typeCount[(int) typeIndex][0]].Source = _cards[typeCount[(int) typeIndex][0]].image;
                await Task.Delay(600);
                _cardImages[typeCount[(int) typeIndex][1]].Source = _cards[typeCount[(int) typeIndex][1]].image;
                await Task.Delay(500);
            }

            if (_shownCards.ContainsKey(cardOne)) _shownCards.Remove(cardOne);

            if (_shownCards.ContainsKey(cardTwo)) _shownCards.Remove(cardTwo);

            _grid.Children.Remove(_cardImages[cardOne]);
            _cardImages.Remove(cardOne);

            _grid.Children.Remove(_cardImages[cardTwo]);
            _cardImages.Remove(cardTwo);

            _shownCards.Remove(cardOne);
            _shownCards.Remove(cardTwo);

            SystemSounds.Asterisk.Play();
            _player2Score++;
            UpdateCurrentPlayer();

            if (_grid.Children.Count > 0) await ComputerAgent();
        }

        /// <summary>
        ///     Picks a random key (card) for computer agent.
        /// </summary>
        /// <param name="knownCards">give all the cards the agent knows of</param>
        /// <param name="retryKey">the key of the card that was previously picked</param>
        /// <returns>a randomly picked key of a card that has not been picked prior</returns>
        private int PickRandomKey(Dictionary<int, List<int>> knownCards, int? retryKey = null)
        {
            var genericCardList = knownCards.SelectMany(c => c.Value).ToList();
            var key = 0;
            var keyList = new List<int>(_cardImages.Keys);
            do
            {
                var rand = new Random();
                key = keyList[rand.Next(keyList.Count)];
            } while (key == retryKey || genericCardList.Any(d => d == key));

            return key;
            
        }
    }
}