using Microsoft.AspNetCore.SignalR;
using server.Data;
using server.Helpers.Enums;
using server.Models;
using server.Models.Dto;
using server.Services;

namespace server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IServerCache _serverCache;
        private readonly IGameService _gameService;

        public GameHub(IServerCache serverCache, IGameService gameService)
        {
            _serverCache = serverCache;
            _gameService = gameService;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnect: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinServer(string serverId, string playerName)
        {
            var (playerId, response) = await _gameService.JoinServerAsync(serverId, playerName, Context.ConnectionId);
            if (playerId is null || response is null)
            {
                LogError("Failed to join server", serverId, Context.ConnectionId);
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, serverId);

            await Clients.Client(Context.ConnectionId).SendAsync("PlayerId", playerId);

            await Clients.Group(serverId).SendAsync("GameUpdate", response);

            Console.WriteLine($"Player joined the game: {Context.ConnectionId}");
        }

        public async Task StartGame(string serverId)
        {
            var response = await _gameService.StartGameAsync(serverId, Context.ConnectionId);
            if (response is null)
            {
                LogError("Failed to start game", serverId, Context.ConnectionId);
                return;
            }

            await Clients.Group(serverId).SendAsync("GameUpdate", response);

            Console.WriteLine($"Game starting...");
        }

        public async Task LeaveServer(string serverId)
        {
            var response = await _gameService.LeaveServerAsync(serverId, Context.ConnectionId);
            if (response is null)
            {
                LogError("Failed to leave server", serverId, Context.ConnectionId);
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId);

            await Clients.Group(serverId).SendAsync("GameUpdate", response);

            Console.WriteLine($"Player left the game: {Context.ConnectionId}");
        }

        public async Task PerformAction(ActionRequestDto request)
        {
            var response = await _gameService.PerformActionAsync(request, Context.ConnectionId);
            if (response is null)
            {
                LogError("Failed to perform action", request.ServerId, Context.ConnectionId);
                return;
            }

            await Clients.Group(request.ServerId).SendAsync("GameUpdate", response);
        }

        private void LogError(string message, string serverId, string socketId)
        {
            Console.WriteLine("----------");
            Console.WriteLine(message);
            Console.WriteLine($"Server: {serverId}");
            Console.WriteLine($"Player: {socketId}");
            Console.WriteLine("----------");
        }
    }
}
