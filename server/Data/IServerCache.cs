using server.Models;

namespace server.Data
{
    public interface IServerCache
    {
        void CreateServer(Server server);
        void DeleteServer(string serverId);
        bool ServerExists(string serverId);
        Server? GetServer(string serverId);
        void UpdateServer(Server server);
    }
}
