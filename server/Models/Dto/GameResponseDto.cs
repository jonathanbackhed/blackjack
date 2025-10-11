using server.Helpers.Enums;

namespace server.Models.Dto
{
    public class GameResponseDto
    {
        public int ServerId { get; set; }

        public GameStatus Status { get; set; }

        public List<PlayerStateDto> Players { get; set; } = new();

        public required string CurrentTurnPlayerId { get; set; }

        //public string Winner { get; set; }

        public string? Message { get; set; }
    }
}
