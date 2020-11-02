using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.Json;
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
        private const double CardScaleHeight = 2;
        private const double CardScaleWidth = 2;
        private const bool GridLines = false;
        private readonly Dictionary<int, Image> _cardImages = new Dictionary<int, Image>();
        private readonly Grid _grid = new Grid();
        private readonly TextBlock _player1Text = new TextBlock();
        private readonly TextBlock _player2Text = new TextBlock();
        private readonly List<int> _selectedCards = new List<int>();
        private List<Card> _cards;
        private Uri _defaultCardImage;
        private List<int> _hiddenCards = new List<int>();
        private int _player1Score;

        private bool _player1Turn = true;
        private int _player2Score;

        private bool _player2Turn;
        private int? _saveId;
        private Dictionary<int, int> _shownCards = new Dictionary<int, int>();
        private string _theme;
        public int GameSize;

        public bool Multiplayer;
        public string Player1Name;
        public string Player2Name;

        /// <summary>
        ///     This initialize the Components
        /// </summary>
        public GamePage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Used to start games and score generation.
        /// </summary>
        /// <param name="multiplayer">a boolean if the game is multiplayer</param>
        /// <param name="gameSize">the gameSize as a int</param>
        /// <param name="saveId">the id of the save game but isn't required</param>
        /// <param name="playerOne">the name of player 1</param>
        /// <param name="playerTwo">the name of player 2</param>
        public void Start(int? saveId = null, bool multiplayer = false, int gameSize = 0, string playerOne = "",
            string playerTwo = "")
        {
            Multiplayer = multiplayer;
            GameSize = gameSize * gameSize;
            Player1Name = playerOne;
            Player2Name = playerTwo;
            _saveId = saveId;
            _theme = App.GetInstance().Theme;

            _defaultCardImage =
                new Uri(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{_theme}/default.jpg");
            if (_saveId != null)
            {
                LoadSave();
            }
            else
            {
                var images =
                    Directory.GetFiles(
                        $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{_theme}/cards",
                        "*");
                images = images.ToList().Take(GameSize / 2).ToArray();
                _cards = Card.Generate(images);
            }

            GenerateScore();
            ShowCards();
        }

        /// <summary>
        ///     Save the current game stat.
        /// </summary>
        public void Save()
        {
            var players = new Dictionary<int, Dictionary<string, dynamic>>
            {
                [0] = new Dictionary<string, dynamic>
                    {{"turn", _player1Turn}, {"score", _player1Score}, {"name", Player1Name}},
                [1] = new Dictionary<string, dynamic>
                    {{"turn", _player2Turn}, {"score", _player2Score}, {"name", Player2Name}}
            };

            var playersJson = JsonSerializer.Serialize(players);
            var shownCardsJson = JsonSerializer.Serialize(_shownCards);
            var cardsJson = JsonSerializer.Serialize(_cards);
            var hiddenCardsJson = JsonSerializer.Serialize(_hiddenCards);
            if (_saveId != null)
            {
                App.GetInstance().Database.Query(
                    $@"UPDATE `saves` SET `multiplayer`={Multiplayer}, `game_size`={GameSize}, `players`='{playersJson}', `cards`='{cardsJson}', `shown_cards`='{shownCardsJson}', `hidden_cards`='{hiddenCardsJson}', `theme`='{_theme}' WHERE `id`={_saveId};");
                return;
            }

            App.GetInstance().Database.Query(
                $@"INSERT INTO `saves` ('multiplayer', 'game_size', 'players', 'cards', 'shown_cards', 'hidden_cards', 'theme') 
                VALUES ({Multiplayer}, {GameSize}, '{playersJson}', '{cardsJson}', '{shownCardsJson}', '{hiddenCardsJson}', '{_theme}');");
        }

        /// <summary>
        ///     Load saved game by _saveId.
        /// </summary>
        public void LoadSave()
        {
            App.GetInstance().Database.Query($@"SELECT * FROM `saves` WHERE `id`={_saveId};", reader =>
            {
                reader.Read();
                Multiplayer = Convert.ToBoolean(reader["multiplayer"]);
                GameSize = Convert.ToInt32(reader["game_size"]);
                _cards = JsonSerializer.Deserialize<List<Card>>(Convert.ToString(reader["cards"]));
                _shownCards = JsonSerializer.Deserialize<Dictionary<int, int>>(Convert.ToString(reader["shown_cards"]));
                _theme = Convert.ToString(reader["theme"]);
                _hiddenCards = JsonSerializer.Deserialize<List<int>>(Convert.ToString(reader["hidden_cards"]));
                var players =
                    JsonSerializer.Deserialize<Dictionary<int, Dictionary<string, dynamic>>>(
                        Convert.ToString(reader["players"]));
                _player1Turn = Convert.ToBoolean(players[0]["turn"].ToString());
                _player1Score = Convert.ToInt32(players[0]["score"].ToString());
                Player1Name = players[0]["name"].ToString();
                _player2Turn = Convert.ToBoolean(players[1]["turn"].ToString());
                _player2Score = Convert.ToInt32(players[1]["score"].ToString());
                Player2Name = players[1]["name"].ToString();
            });
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
            if (_player1Turn)
            {
                _player1Text.Foreground = new SolidColorBrush(Colors.Red);
                _player2Text.Foreground = new SolidColorBrush(Colors.Black);
            }

            if (_player2Turn)
            {
                _player1Text.Foreground = new SolidColorBrush(Colors.Black);
                _player2Text.Foreground = new SolidColorBrush(Colors.Red);
            }

            _player1Text.Text = $"{Player1Name}: {_player1Score}";

            // var type = _multiplayer ? "Player 2" : "Computer";
            _player2Text.Text = $"{Player2Name}: {_player2Score}";
        }

        /// <summary>
        ///     Generates playing area.
        /// </summary>
        private void ShowCards()
        {
            var rows = Math.Sqrt(GameSize);
            var columns = Math.Sqrt(GameSize);

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
                if (_hiddenCards.Contains(index))
                {
                    index++;
                    continue;
                }

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
            if (!Multiplayer && _player2Turn)
            {
                Func<Task> computerAgent = ComputerAgent;
                computerAgent();
            }
        }

        /// <summary>
        ///     Button handler to detect card selected.
        /// </summary>
        /// <param name="card">the card that has been clicked</param>
        /// <param name="cardImage">the image of the card that needs to be changed</param>
        /// <param name="index">the index of card that has been click</param>
        private async void ButtonHandler(Card card, Image cardImage, int index)
        {
            if (!Multiplayer && _player2Turn) return;
            if (_selectedCards.Count >= 1 && _selectedCards[0] == index || _selectedCards.Count == 2) return;
            cardImage.Source = card.GetImage();
            _selectedCards.Add(index);

            if (!_shownCards.ContainsKey(index)) _shownCards.Add(index, card.Type);

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
            if (Multiplayer)
                App.GetInstance().Database
                    .Query($@"INSERT INTO `scores` ('name', 'score') VALUES ('{Player2Name}', {_player2Score});");

            App.GetInstance().Database
                .Query($@"INSERT INTO `scores` ('name', 'score') VALUES ('{Player1Name}', {_player1Score});");

            if (_saveId != null) App.GetInstance().Database.Query($@"DELETE FROM `saves` WHERE `id`={_saveId};");

            MainWindow.GetMainWindow().ChangePage(MainWindow.GetMainWindow().mainPage);
        }

        /// <summary>
        ///     Check if selected cards are the same.
        /// </summary>
        /// <returns>A task</returns>
        private async Task CheckCards()
        {
            if (_cards[_selectedCards[0]].Type == _cards[_selectedCards[1]].Type)
            {
                if (_player1Turn) _player1Score++;
                if (_player2Turn) _player2Score++;

                await Task.Delay(500);
                _grid.Children.Remove(_cardImages[_selectedCards[0]]);
                _cardImages.Remove(_selectedCards[0]);
                _hiddenCards.Add(_selectedCards[0]);

                _grid.Children.Remove(_cardImages[_selectedCards[1]]);
                _cardImages.Remove(_selectedCards[1]);
                _hiddenCards.Add(_selectedCards[1]);

                _shownCards.Remove(_selectedCards[0]);
                _shownCards.Remove(_selectedCards[1]);

                SystemSounds.Asterisk.Play();
                UpdateCurrentPlayer();
                return;
            }

            await Task.Delay(1000);
            _cardImages[_selectedCards[0]].Source = _cardImages[_selectedCards[1]].Source =
                new BitmapImage(_defaultCardImage);

            _player1Turn = !_player1Turn;
            _player2Turn = !_player2Turn;

            SystemSounds.Hand.Play();
            UpdateCurrentPlayer();
            if (!Multiplayer) await ComputerAgent();
        }

        /// <summary>
        ///     Start turn of the computer agent.
        /// </summary>
        /// <returns>A task</returns>
        private async Task ComputerAgent()
        {
            var typeCount = new Dictionary<int, List<int>>();
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
                cardOne = await PickRandomKey(typeCount);
                cardTwo = await PickRandomKey(typeCount, cardOne);

                if (!_shownCards.ContainsKey(cardOne)) _shownCards.Add(cardOne, _cards[cardOne].Type);

                if (!_shownCards.ContainsKey(cardTwo)) _shownCards.Add(cardTwo, _cards[cardTwo].Type);

                await Task.Delay(600);
                _cardImages[cardOne].Source = _cards[cardOne].GetImage();
                await Task.Delay(600);
                _cardImages[cardTwo].Source = _cards[cardTwo].GetImage();
                await Task.Delay(500);
                if (_cards[cardOne].Type != _cards[cardTwo].Type)
                {
                    _cardImages[cardOne].Source = _cardImages[cardTwo].Source = new BitmapImage(_defaultCardImage);
                    _player1Turn = !_player1Turn;
                    _player2Turn = !_player2Turn;
                    SystemSounds.Hand.Play();
                    UpdateCurrentPlayer();
                    if (_grid.Children.Count <= 0) GameFinished();
                    return;
                }
            }
            else
            {
                cardOne = typeCount[(int) typeIndex][0];
                cardTwo = typeCount[(int) typeIndex][1];
                await Task.Delay(600);
                _cardImages[typeCount[(int) typeIndex][0]].Source = _cards[typeCount[(int) typeIndex][0]].GetImage();
                await Task.Delay(600);
                _cardImages[typeCount[(int) typeIndex][1]].Source = _cards[typeCount[(int) typeIndex][1]].GetImage();
                await Task.Delay(500);
            }

            if (_shownCards.ContainsKey(cardOne)) _shownCards.Remove(cardOne);

            if (_shownCards.ContainsKey(cardTwo)) _shownCards.Remove(cardTwo);

            _grid.Children.Remove(_cardImages[cardOne]);
            _cardImages.Remove(cardOne);
            _hiddenCards.Add(cardOne);

            _grid.Children.Remove(_cardImages[cardTwo]);
            _cardImages.Remove(cardTwo);
            _hiddenCards.Add(cardTwo);

            _shownCards.Remove(cardOne);
            _shownCards.Remove(cardTwo);

            SystemSounds.Asterisk.Play();
            _player2Score++;
            UpdateCurrentPlayer();

            if (_grid.Children.Count > 0) await ComputerAgent();
            if (_grid.Children.Count <= 0) GameFinished();
        }

        /// <summary>
        ///     Picks a random key (card) for computer agent.
        /// </summary>
        /// <param name="knownCards">give all the cards the agent knows of</param>
        /// <param name="retryKey">the key of the card that was previously picked</param>
        /// <returns>a randomly picked key of a card that has not been picked prior</returns>
        private async Task<int> PickRandomKey(Dictionary<int, List<int>> knownCards, int? retryKey = null)
        {
            var genericCardList = knownCards.SelectMany(c => c.Value).ToList();
            var key = 0;
            var keyList = new List<int>(_cardImages.Keys);
            do
            {
                var rand = new Random();
                key = keyList[rand.Next(keyList.Count)];
                await Task.Delay(20);
            } while (key == retryKey || genericCardList.Any(d => d == key));

            return key;
        }
    }
}