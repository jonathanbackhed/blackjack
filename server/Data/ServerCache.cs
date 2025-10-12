using Microsoft.Extensions.Caching.Memory;
using server.Models;

namespace server.Data
{
    public class ServerCache : IServerCache
    {
        private readonly IMemoryCache _cache;

        public ServerCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void CreateServer(Server server)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1);
            _cache.Set(server.Id.ToString(), server, cacheEntryOptions);
        }

        public void DeleteServer(string serverId)
        {
            _cache.Remove(serverId);
        }

        public bool ServerExists(string serverId)
        {
            _cache.TryGetValue(serverId, out Server? server);
            if (server is null)
                return false;

            return true;
        }

        public Server? GetServer(string serverId)
        {
            _cache.TryGetValue(serverId, out Server? server);
            return server;
        }

        public void UpdateServer(Server newServer)
        {
            _cache.TryGetValue(newServer.Id.ToString(), out Server? server);
            if (server is not null)
            {
                _cache.Set(newServer.Id.ToString(), newServer);
            }
        }
    }
}
