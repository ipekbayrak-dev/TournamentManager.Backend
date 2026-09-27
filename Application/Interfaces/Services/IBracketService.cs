using TournamentManager.Application.Common;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IBracketService
    {
        Task<Result> GenerateBracketAsync(Guid tournamentId, CancellationToken cancellationToken = default);
    }
}
