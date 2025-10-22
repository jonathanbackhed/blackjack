using server.Data;
using server.Helpers;
using server.Models;

namespace server.Services
{
    public class CacheInitializer
    {
        private readonly IServerCache _serverCache;

        public CacheInitializer(IServerCache serverCache)
        {
            _serverCache = serverCache;
        }

        public void Initialize()
        {
            var server = new Server
            {
                Id = new Guid("8bd355ca-aea4-41bf-b937-a4ff84002e1a"),
                GameCode = "9QDtGDV5",
                MaxPlayerCount = 3
            };

            _serverCache.CreateServer(server);
        }
    }
}
