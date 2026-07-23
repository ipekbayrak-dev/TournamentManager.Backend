using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Player;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IPlayerService
    {
        public Task<Result<PlayerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<PlayerResponse>>> GetAllByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default);
        public Task<Result<PlayerResponse>> CreateAsync(CreatePlayerRequest createPlayerRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdatePlayerRequest updatePlayerRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}