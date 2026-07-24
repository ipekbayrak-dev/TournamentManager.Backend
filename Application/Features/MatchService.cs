using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class MatchService : IMatchService
    {
        public Task<Result<MatchResponse>> CreateAsync(CreateMatchRequest createMatchRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<MatchResponse>>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<MatchResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdateMatchRequest updateMatchRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}