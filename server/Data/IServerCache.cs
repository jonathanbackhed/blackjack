using server.Models;

namespace server.Data
{
    public interface IServerCache
    {
        void CreateServer(Server server);
        void RemoveServer(string gameCode);
        bool ServerExists(string gameCode);
        Server? GetServer(string gameCode);
    }
}
