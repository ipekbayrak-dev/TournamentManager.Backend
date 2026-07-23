using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Prize;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IPrizeService
    {
        public Task<Result<PrizeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<PrizeResponse>> GetByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default);
        public Task<Result<PrizeResponse>> CreateAsync(CreatePrizeRequest createPrizeRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdatePrizeRequest updatePrizeRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}