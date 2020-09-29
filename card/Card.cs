using System;
using Memory;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Memory.card
{
    /// <summary>
    /// Object used for handling the game card mechanics.
    /// </summary>
    class Card
    {
        public readonly int Type;
        public readonly BitmapImage Image;

        private Card(int type, string path)
        {
            Type = type;

            /*
            Uri uri = new Uri(path);
            Image = new BitmapImage(uri);
            */
            Image = null;
        }

        /// <summary>
        /// Used for generating a random list of cards.
        /// </summary>
        /// <param name="images">The image paths to base the cards off of.</param>
        /// <returns>A list of cards twice as big as images.</returns>
        public static List<Card> Generate(string[] images)
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < images.Length; i++)
            {
                // Add 2 card objects to list
                cards.Add(new Card(i, images[i]));
                cards.Add(new Card(i, images[i]));
            }

            Shuffle(cards);
            return cards;
        }

        /// <summary>
        /// Shuffles a list of cards.
        /// </summary>
        /// <param name="cards">The List of cards to shuffle.</param>
        public static void Shuffle(IList<Card> cards)
        {
            Random random = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                int j = random.Next(i);
                Card value = cards[j];
                cards[j] = cards[i];
                cards[i] = value;
            }
        }
    }
}
