using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string SocketId { get; set; }

        public required string Name { get; set; }

        public int WinLossRatio { get; set; } = 0;

        public Hand Hand { get; set; } = new();

        public bool IsStanding { get; set; } = false;

        public bool IsDealer { get; set; } = false;
    }
}
