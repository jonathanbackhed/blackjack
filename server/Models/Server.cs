using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Server
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(8)]
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public required string GameCode { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime? Closed { get; set; }

        [NotMapped]
        public int MaxPlayerCount { get; set; }

        [NotMapped]
        public bool IsStarted { get; set; } = false;

        [NotMapped]
        public List<Player> Players { get; set; } = new List<Player>
        {
            new Player {SocketId = "", Name = "Dealer", IsDealer = true}
        };

        [NotMapped]
        public Deck Deck { get; set; } = new();

        public void AddPlayer(Player player)
        {
            if (!IsStarted && (Players.Count - 1) < MaxPlayerCount)
                Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }

        public void StartGame()
        {
            if (Players.Count == 1) return;
            IsStarted = true;

            foreach (var player in Players)
            {
                player.Hand.Cards.Add(Deck.DrawCard());
            }

            foreach (var player in Players)
            {
                player.Hand.Cards.Add(Deck.DrawCard());
            }
        }

        public void DealerTurn()
        {
            var dealer = Players.FirstOrDefault(p => p.IsDealer);
            if (dealer == null) return;

            while (dealer.Hand.GetValue() < 17)
            {
                dealer.Hand.Cards.Add(Deck.DrawCard());
            }
        }

        public string DetermineWinner()
        {
            var dealer = Players.FirstOrDefault(p => p.IsDealer);
            int dealerValue = dealer!.Hand.GetValue();

            var winners = Players
                .Where(p => !p.IsDealer && !p.Hand.IsBust && p.Hand.GetValue() > dealerValue || dealer.Hand.IsBust)
                .ToList();

            return winners.Count > 0 ? string.Join(", ", winners.Select(p => p.Name)) : "Dealer";
        }
    }
}
