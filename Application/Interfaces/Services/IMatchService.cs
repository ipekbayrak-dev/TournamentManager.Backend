using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Match;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IMatchService
    {
        public Task<Result<MatchResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<MatchResponse>>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default);
        public Task<Result<MatchResponse>> CreateAsync(CreateMatchRequest createMatchRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdateMatchRequest updateMatchRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}