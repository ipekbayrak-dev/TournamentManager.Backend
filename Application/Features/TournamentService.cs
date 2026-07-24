using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Tournament;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class TournamentService : ITournamentService
    {
        public Task<Result<TournamentResponse>> CreateAsync(CreateTournamentRequest createTournamentRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<TournamentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TournamentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdateTournamentRequest updateTournamentRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}