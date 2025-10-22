using server.Models;
using server.Models.Dto;

namespace server.Services
{
    public interface IGameService
    {
        Task<(string? PlayerId, GameResponseDto? Response)> JoinServerAsync(string serverId, string playerName, string socketId);
        Task<GameResponseDto?> LeaveServerAsync(string serverId, string socketId);
        Task<GameResponseDto?> StartGameAsync(string serverId, string socketId);
        Task<GameResponseDto?> PerformActionAsync(ActionRequestDto request, string socketId);
    }
}
