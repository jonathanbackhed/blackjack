namespace server.Models.Dto
{
    public class PlayerStateDto
    {
        public required string PlayerId { get; set; }

        public required string Name { get; set; }

        public List<Card> Cards { get; set; }

        public int HandValue { get; set; }

        public bool IsDealer { get; set; }

        public bool IsStanding { get; set; }

        public bool IsBust { get; set; }
    }
}
