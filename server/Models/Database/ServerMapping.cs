using System.ComponentModel.DataAnnotations;

namespace server.Models.Database
{
    public class ServerMapping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(8)]
        public required string GameCode { get; set; }

        [Required]
        public required string ServerId { get; set; }

        [Required]
        public bool IsInactive { get; set; } = false;
    }
}
