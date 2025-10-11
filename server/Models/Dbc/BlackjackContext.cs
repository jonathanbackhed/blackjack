using Microsoft.EntityFrameworkCore;

namespace server.Models.Dbc
{
    public class BlackjackContext : DbContext
    {
        public BlackjackContext(DbContextOptions<BlackjackContext> options) : base(options)
        {
        }

        public DbSet<Server> Servers { get; set; }
    }
}
