using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Memory.card
{
    /// <summary>
    ///     Object used for handling the game card mechanics.
    /// </summary>
    internal class Card
    {
        private Card(int type, string path)
        {
            Type = type;
            Uri = new Uri(path);
        }

        public Card()
        {
        }

        public Uri Uri { get; set; }
        public int Type { get; set; }

        /// <summary>
        ///     Used for generating a random list of cards.
        /// </summary>
        /// <param name="images">The image paths to base the cards off of.</param>
        /// <returns>A list of cards twice as big as images.</returns>
        public static List<Card> Generate(string[] images)
        {
            var cards = new List<Card>();

            for (var i = 0; i < images.Length; i++)
            {
                // Add 2 card objects to list
                cards.Add(new Card(i, images[i]));
                cards.Add(new Card(i, images[i]));
            }

            Shuffle(cards);
            return cards;
        }

        /// <summary>
        ///     Shuffles a list of cards.
        /// </summary>
        /// <param name="cards">The List of cards to shuffle.</param>
        private static void Shuffle(IList<Card> cards)
        {
            var random = new Random();

            for (var i = 0; i < cards.Count; i++)
            {
                var j = random.Next(i);
                var value = cards[j];
                cards[j] = cards[i];
                cards[i] = value;
            }
        }

        public BitmapImage GetImage()
        {
            return new BitmapImage(Uri);
        }
    }
}