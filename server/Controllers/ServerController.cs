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

        public ServerController(IServerCache serverCache)
        {
            _serverCache = serverCache;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateServer()
        {
            var server = new Server { GameCode = GenerateCode.GenerateGameCode() };
            while (_serverCache.ServerExists(server.GameCode))
            {
                server.GameCode = GenerateCode.GenerateGameCode();
            }

            _serverCache.CreateServer(server);

            return Ok(server.GameCode);
        }

        [HttpGet("get/{gameCode}")]
        public async Task<IActionResult> GetServer([FromRoute] string gameCode)
        {
            var server = _serverCache.GetServer(gameCode);

            if (server is null) return BadRequest("Server not found!");

            return Ok(server);
        }
    }
}
