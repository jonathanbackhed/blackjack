using server.Helpers.Enums;
using TypeGen.Core.TypeAnnotations;

namespace server.Models.Dto
{
    [ExportTsInterface]
    public class GameResponseDto
    {
        public required string ServerId { get; set; }

        public ServerStatus Status { get; set; }

        public required List<PlayerStateDto> Players { get; set; }

        public required string CurrentTurnPlayerId { get; set; }

        public string? Winner { get; set; }

        public string? Message { get; set; }
    }
}
