using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.TournamentEntry;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class TournamentEntryService : ITournamentEntryService
    {
        public Task<Result<TournamentEntryResponse>> CreateAsync(CreateTournamentEntryRequest createTournamentEntryRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<TournamentEntryResponse>>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TournamentEntryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdateTournamentEntryRequest updateTournamentEntryRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}