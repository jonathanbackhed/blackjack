using TypeGen.Core.TypeAnnotations;

namespace server.Models
{
    [ExportTsInterface]
    public class Card
    {
        public required string Suit { get; set; } // Hearts, Diamonds, Clubs, Spades

        public required string Rank { get; set; } // 2-10, J, Q, K, A

        [TsIgnore]
        public int Value
        {
            get
            {
                if (this.Rank == "A") return 11; // Add logic for 1 or 11
                if (this.Rank == "K" || this.Rank == "Q" || this.Rank == "J") return 10;
                return int.Parse(this.Rank);
            }
        }
    }
}
