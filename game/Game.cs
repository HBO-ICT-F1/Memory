using System.Collections.Generic;
using System.IO;
using System.Linq;
using Memory.card;
using Memory.game.players;

namespace Memory.game
{
    /// <summary>
    ///     Game class
    /// </summary>
    public class Game
    {
        private readonly List<Card> _cards;
        private readonly List<Player> _players;
        private readonly string _theme;
        private int _turn;

        public Game(Player opponent, int gameSize, string theme)
        {
            var user = new ClientPlayer("User");
            _players = new List<Player> {user, opponent};

            var images = GetImages(gameSize);
            _cards = Card.Generate(images);
            _theme = theme;
        }

        /// <summary>
        ///     Gets the paths to images for the currently selected theme
        /// </summary>
        /// <param name="gameSize">The size of the game</param>
        /// <returns>An array of image paths</returns>
        private string[] GetImages(int gameSize)
        {
            var images =
                Directory.GetFiles(
                    $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/ui/assets/themes/{_theme}/cards",
                    "*");
            return images.ToList().Take(gameSize / 2).ToArray();
        }

        /// <summary>
        ///     Update the game state
        /// </summary>
        public void Update()
        {
            var visible = _cards.FindAll(c => c.Visible);
            if (visible.Count >= 2)
            {
                var equals = visible.All(first => visible.Any(second => first.Type == second.Type));

                if (equals)
                    // TODO: Score
                    visible.FindAll(c => c.Shown).ForEach(c => c.Visible = false);
                else
                    visible.ForEach(c => c.Shown = false);

                _turn++;
                return;
            }

            var player = _players[_turn];
            var move = player.Move(_cards);
            var card = _cards[move];
            card.Shown = true;
            card.Known = true;
        }
    }
}