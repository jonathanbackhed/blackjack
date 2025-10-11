namespace server.Models
{
    public class Hand
    {
        public List<Card> Cards { get; set; } = new();

        public int GetValue()
        {
            int value = Cards.Sum(c => c.Value);
            int aceCount = Cards.Count(c => c.Rank == "A");

            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }

        public bool IsBust => GetValue() > 21;
    }
}
