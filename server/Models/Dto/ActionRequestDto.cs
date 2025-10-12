using server.Helpers.Enums;

namespace server.Models.Dto
{
    public class ActionRequestDto
    {
        public required string ServerId { get; set; }

        public required string PlayerId { get; set; }

        public required PlayerAction Action { get; set; }
    }
}
