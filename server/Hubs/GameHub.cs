using Microsoft.AspNetCore.SignalR;
using server.Data;
using server.Helpers.Enums;
using server.Models;
using server.Models.Dto;

namespace server.Hubs
{
    public class GameHub : Hub
    {
        //private static Dictionary<string, Server> _servers = new();
        private readonly IServerCache _serverCache;

        public GameHub(IServerCache serverCache)
        {
            _serverCache = serverCache;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public async Task JoinServer(string serverId, string playerName)
        {
            //if (!_servers.ContainsKey(serverId)) return;
            if (!_serverCache.ServerExists(serverId)) return;

            //var server = _servers[serverId];
            var server = _serverCache.GetServer(serverId)!;

            var player = new Player()
            {
                SocketId = Context.ConnectionId,
                Name = playerName
            };

            server.AddPlayer(player);

            await Groups.AddToGroupAsync(Context.ConnectionId, serverId);

            await Clients.Group(serverId).SendAsync("GameUpdate", MapToServerResponse(server));
        }

        public async Task PerformAction(ActionRequestDto request)
        {
            //if (!_servers.ContainsKey(request.ServerId)) return;
            if (!_serverCache.ServerExists(request.ServerId)) return;

            //var server = _servers[request.ServerId];
            var server = _serverCache.GetServer(request.ServerId)!;
            var player = server.Players.FirstOrDefault(p => p.Id.ToString() == request.PlayerId);
            if (player is null) return;

            switch (request.Action)
            {
                case PlayerAction.Hit:
                    player.Hand.Cards.Add(server.Deck.DrawCard());
                    if (player.Hand.IsBust)
                        player.IsStanding = true;
                    break;

                case PlayerAction.Stand:
                    player.IsStanding = true;
                    break;

                case PlayerAction.Double:
                    throw new InvalidOperationException("Not implemented yet");
                    break;

                case PlayerAction.Leave:
                    throw new InvalidOperationException("Not implemented yet");
                    break;
            }

            if (server.Players.Where(p => !p.IsDealer).All(p => p.IsStanding))
            {
                server.DealerTurn();
                var winner = server.DetermineWinner();
                var finished = MapToServerResponse(server);
                finished.Status = ServerStatus.Finished;
                finished.Winner = winner;

                await Clients.Group(request.ServerId).SendAsync("GameUpdate", finished);
                return;
            }

            await Clients.Group(request.ServerId).SendAsync("GameUpdate", MapToServerResponse(server));
        }

        private GameResponseDto MapToServerResponse(Server server)
        {
            var currentPlayer = server.Players.FirstOrDefault(p => !p.IsDealer && !p.IsStanding);
            return new GameResponseDto
            {
                ServerId = server.Id.ToString(),
                Status = server.IsStarted ? ServerStatus.InProgress : ServerStatus.Waiting,
                CurrentTurnPlayerId = currentPlayer!.Id.ToString(),
                Players = server.Players.Select(p => new PlayerStateDto
                {
                    PlayerId = p.Id.ToString(),
                    Name = p.Name,
                    Cards = p.IsDealer && server.IsStarted && server.Players.Any(pl => !pl.IsStanding)
                        ? new List<Card> { p.Hand.Cards.First(), new Card { Suit = "Hidden", Rank = "?" } }
                        : p.Hand.Cards,
                    HandValue = p.Hand.GetValue(),
                    IsDealer = p.IsDealer,
                    IsStanding = p.IsStanding,
                    IsBust = p.Hand.IsBust
                }).ToList()
            };
        }
    }
}
