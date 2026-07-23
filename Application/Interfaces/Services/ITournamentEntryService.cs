using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.TournamentEntry;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface ITournamentEntryService
    {
        public Task<Result<TournamentEntryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<TournamentEntryResponse>>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default);
        public Task<Result<TournamentEntryResponse>> CreateAsync(CreateTournamentEntryRequest createTournamentEntryRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdateTournamentEntryRequest updateTournamentEntryRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}