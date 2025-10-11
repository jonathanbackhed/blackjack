using server.Models;

namespace server.Data
{
    public interface IServerRepository
    {
        Task<bool> CreateServer(Server server);
    }
}
