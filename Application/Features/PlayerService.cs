using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Player;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class PlayerService : IPlayerService
    {
        public Task<Result<PlayerResponse>> CreateAsync(CreatePlayerRequest createPlayerRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<PlayerResponse>>> GetAllByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PlayerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdatePlayerRequest updatePlayerRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}