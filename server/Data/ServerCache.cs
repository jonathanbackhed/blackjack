using Microsoft.Extensions.Caching.Memory;
using server.Models;

namespace server.Data
{
    public class ServerCache : IServerCache
    {
        private MemoryCache _cache { get; set; }

        public ServerCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 10});
        }

        public void CreateServer(Server server)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1);
            _cache.Set(server.GameCode, server, cacheEntryOptions);
        }

        public void RemoveServer(string gameCode)
        {
            _cache.Remove(gameCode);
        }

        public bool ServerExists(string gameCode)
        {
            _cache.TryGetValue(gameCode, out Server? server);
            if (server is null)
                return false;

            return true;
        }

        public Server? GetServer(string gameCode)
        {
            _cache.TryGetValue(gameCode, out Server? server);
            return server;
        }
    }
}
