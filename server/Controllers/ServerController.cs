using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Helpers;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IServerCache _serverCache;
        private readonly IServerRepository _serverRepository;

        public ServerController(IServerCache serverCache, IServerRepository serverRepository)
        {
            _serverCache = serverCache;
            _serverRepository = serverRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateServer()
        {
            var server = new Server 
            { 
                GameCode = GenerateCode.GenerateGameCode(),
                MaxPlayerCount = 3
            };

            while (_serverCache.ServerExists(server.GameCode))
            {
                server.GameCode = GenerateCode.GenerateGameCode();
            }

            _serverCache.CreateServer(server);
            await _serverRepository.CreateServerMappingAsync(server);

            return Ok(server.GameCode);
        }

        [HttpGet("getid/{gameCode}")]
        public async Task<IActionResult> GetServerIdFromCode([FromRoute] string gameCode)
        {
            var serverId = await _serverRepository.GetServerIdByGameCodeAsync(gameCode);
            if (serverId is null) return BadRequest("Server not found!");

            return Ok(serverId);
        }

        [HttpGet("get/{serverId}")]
        public async Task<IActionResult> GetServer([FromRoute] string serverId)
        {
            var server = _serverCache.GetServer(serverId);

            if (server is null) return BadRequest("Server not found!");

            return Ok(server);
        }
    }
}
