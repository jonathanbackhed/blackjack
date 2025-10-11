namespace server.Models
{
    public class Deck
    {
        private Stack<Card> _cards;
        private static readonly string[] Suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        private static readonly string[] Ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        public Deck()
        {
            var cards = new List<Card>();

            for (int i = 0; i < 1; i++)
            {
                cards.AddRange(GetFullDeck());
            }

            ShuffleDeck(cards);
            _cards = new Stack<Card>(cards);
        }

        public Card DrawCard() => _cards.Pop();

        private List<Card> GetFullDeck()
        {
            var cards = new List<Card>();

            foreach (var suit in Suits)
            {
                foreach (var rank in Ranks)
                {
                    cards.Add(new Card { Suit = suit, Rank = rank });
                }
            }

            return cards;
        }

        private void ShuffleDeck(List<Card> cards)
        {
            var rng = new Random(Guid.NewGuid().GetHashCode());
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (cards[k], cards[n]) = (cards[n], cards[k]);
            }
        }
    }
}
