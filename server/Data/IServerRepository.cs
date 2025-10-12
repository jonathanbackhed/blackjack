using server.Models;

namespace server.Data
{
    public interface IServerRepository
    {
        Task CreateServerMappingAsync(Server server);
        Task DeleteServerMappingAsync(string serverId);
        Task<string?> GetServerIdByGameCodeAsync(string gameCode);
    }
}
