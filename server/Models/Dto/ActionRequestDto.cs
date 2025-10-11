using server.Helpers.Enums;

namespace server.Models.Dto
{
    public class ActionRequestDto
    {
        public int ServerId { get; set; }

        public int PlayerId { get; set; }

        public PlayerAction Action { get; set; }

        public object? Payload { get; set; } // For future use like bet amount
    }
}
