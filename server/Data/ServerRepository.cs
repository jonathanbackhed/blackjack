using Microsoft.EntityFrameworkCore;
using server.Helpers;
using server.Models;
using server.Models.Database;

namespace server.Data
{
    public class ServerRepository : IServerRepository
    {
        private readonly BlackjackContext _dbc;

        public ServerRepository(BlackjackContext dbc)
        {
            _dbc = dbc;
        }

        public async Task CreateServerMappingAsync(Server server)
        {
            var mapping = new ServerMapping()
            {
                GameCode = server.GameCode,
                ServerId = server.Id.ToString()
            };

            await _dbc.ServerMappings.AddAsync(mapping);
            await _dbc.SaveChangesAsync();
        }

        public async Task DeleteServerMappingAsync(string serverId)
        {
            var mapping = await _dbc.ServerMappings.FirstOrDefaultAsync(m => m.ServerId == serverId);
            if (mapping is null) return;

            mapping.IsInactive = true;
            _dbc.ServerMappings.Update(mapping);
            await _dbc.SaveChangesAsync();
        }

        public async Task<string?> GetServerIdByGameCodeAsync(string gameCode)
        {
            var mapping = await _dbc.ServerMappings.FirstOrDefaultAsync(m => m.GameCode == gameCode && !m.IsInactive);

            return mapping?.ServerId;
        }
    }
}
