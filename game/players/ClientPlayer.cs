using System.Collections.Generic;
using Memory.card;

namespace Memory.game.players
{
    /// <summary>
    ///     The Player object for the user
    /// </summary>
    public class ClientPlayer : Player
    {
        private Card _card;

        public ClientPlayer(string name) : base(name)
        {
        }

        /// <summary>
        ///     Called on click to handle input
        /// </summary>
        /// <param name="card">The card that was clicked</param>
        public void OnClick(Card card)
        {
            _card = card;
        }

        /// <summary>
        ///     Called to find what move the player wants to make
        /// </summary>
        /// <param name="all">All cards on the field</param>
        /// <returns>The index of the card that the player clicked</returns>
        public override int Move(List<Card> all)
        {
            // Wait for _card to not be null
            while (_card == null)
            {
            }

            // Find the card index
            var index = all.FindIndex(c => c == _card);

            // Reset value and return
            _card = null;
            return index;
        }
    }
}