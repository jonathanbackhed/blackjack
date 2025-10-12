using Microsoft.EntityFrameworkCore;

namespace server.Models.Database
{
    public class BlackjackContext : DbContext
    {
        public BlackjackContext(DbContextOptions<BlackjackContext> options) : base(options)
        {
        }

        public DbSet<ServerMapping> ServerMappings { get; set; }
    }
}
