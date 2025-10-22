using server.Helpers.Enums;
using TypeGen.Core.TypeAnnotations;

namespace server.Models.Dto
{
    [ExportTsInterface]
    public class ActionRequestDto
    {
        public required string ServerId { get; set; }

        public required string PlayerId { get; set; }

        public required PlayerAction Action { get; set; }
    }
}
