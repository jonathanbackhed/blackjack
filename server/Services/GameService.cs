using Microsoft.AspNetCore.SignalR;
using server.Data;
using server.Helpers.Enums;
using server.Hubs;
using server.Models;
using server.Models.Dto;

namespace server.Services
{
    public class GameService : IGameService
    {
        private readonly IServerCache _serverCache;
        private readonly int _msDelay = 2000;

        public GameService(IServerCache serverCache)
        {
            _serverCache = serverCache;
        }

        public async Task<(string? PlayerId, GameResponseDto? Response)> JoinServerAsync(string serverId, string playerName, string socketId)
        {
            var server = _serverCache.GetServer(serverId);
            if (server is null)
            {
                Console.WriteLine("Server is null");
                return (null, null);
            }

            if (server.Players.Any(p => p.SocketId == socketId))
            {
                Console.WriteLine("Client already connected to server");
                return (null, null);
            }

            var player = new Player
            {
                SocketId = socketId,
                Name = playerName
            };

            server.AddPlayer(player);

            var dto = MapToServerResponse(server);

            return (player.Id.ToString(), dto);
        }

        public async Task<GameResponseDto?> LeaveServerAsync(string serverId, string socketId)
        {
            var server = _serverCache.GetServer(serverId);
            if (server is null)
            {
                Console.WriteLine("Server is null");
                return null;
            }

            var player = server.Players.FirstOrDefault(p => p.SocketId == socketId);
            if (player is null)
            {
                Console.WriteLine("Client not found in server");
                return null;
            }

            server.RemovePlayer(player);

            return MapToServerResponse(server);
        }

        public async Task<GameResponseDto?> PerformActionAsync(ActionRequestDto request, string socketId, Func<GameResponseDto, Task>? reportProgress = null, CancellationToken ct = default)
        {
            var server = _serverCache.GetServer(request.ServerId);
            if (server is null)
            {
                Console.WriteLine("Server is null");
                return null;
            }

            var player = server.Players.FirstOrDefault(p => p.Id.ToString() == request.PlayerId);
            if (player is null)
            {
                Console.WriteLine("Client not found in server");
                return null;
            }

            if (player.SocketId.ToString() != socketId)
            {
                Console.WriteLine("Action attempted by invalid client");
                return null;
            }

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

                case PlayerAction.Split:
                    throw new InvalidOperationException("Not implemented yet");

                case PlayerAction.Leave:
                    throw new InvalidOperationException("Not implemented yet");
            }

            if (server.Players.Where(p => !p.IsDealer).All(p => p.IsStanding))
            {
                var dealer = server.Players.FirstOrDefault(p => p.IsDealer);
                if (dealer is null) return null;

                while (dealer.Hand.GetValue() < 17)
                {
                    dealer.Hand.Cards.Add(server.Deck.DrawCard());
                    if (reportProgress is not null)
                        await reportProgress(MapToServerResponse(server));

                    await Task.Delay(_msDelay, ct);
                }

                var winner = server.DetermineWinner();
                var finished = MapToServerResponse(server);
                finished.Status = ServerStatus.Finished;
                finished.Winner = winner;

                return finished;
            }

            return MapToServerResponse(server);
        }

        public async Task<GameResponseDto?> StartGameAsync(string serverId, string socketId)
        {
            var server = _serverCache.GetServer(serverId);
            if (server is null)
            {
                Console.WriteLine("Server is null");
                return null;
            }

            if (!server.Players.Any(p => p.SocketId == socketId))
            {
                Console.WriteLine("Player not in server");
                return null;
            }

            if (!server.IsStarted)
            {
                server.StartGame();
            }
            else if (server.IsStarted && server.Status == ServerStatus.InProgress)
            {
                Console.WriteLine("Round in progress");
                return null;
            }
            else if (server.IsStarted && server.Status == ServerStatus.Finished)
            {
                server.NewRound();
            }

            return MapToServerResponse(server);
        }

        private GameResponseDto MapToServerResponse(Server server)
        {
            var currentPlayer = server.Players.FirstOrDefault(p => !p.IsDealer && !p.IsStanding);
            if (currentPlayer is null)
                currentPlayer = server.Players.FirstOrDefault(p => p.IsDealer);

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
