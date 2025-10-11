using server.Helpers;
using server.Models;
using server.Models.Dbc;

namespace server.Data
{
    public class ServerRepository : IServerRepository
    {
        private readonly BlackjackContext _dbc;

        public ServerRepository(BlackjackContext dbc)
        {
            _dbc = dbc;
        }

        public async Task<bool> CreateServer(Server server)
        {
            server.Closed = DateTime.Now;

            await _dbc.Servers.AddAsync(server);
            var result = await _dbc.SaveChangesAsync();

            return result > 0;
        }
    }
}
