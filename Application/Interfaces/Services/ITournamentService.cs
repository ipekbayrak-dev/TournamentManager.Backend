using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Tournament;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface ITournamentService
    {
        public Task<Result<TournamentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<TournamentResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Result<TournamentResponse>> CreateAsync(CreateTournamentRequest createTournamentRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdateTournamentRequest updateTournamentRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}