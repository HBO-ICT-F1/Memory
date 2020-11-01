using System.Collections.Generic;
using Memory.card;

namespace Memory.game
{
    /// <summary>
    ///     Abstract class for creating different types of players
    /// </summary>
    public abstract class Player
    {
        public readonly string name;

        public Player(string name)
        {
            this.name = name;
        }

        /// <summary>
        ///     Asks the player to make a move
        /// </summary>
        /// <returns>The index of the card that was flipped</returns>
        public abstract int Move(List<Card> all);
    }
}