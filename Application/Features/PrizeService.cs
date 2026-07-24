using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Prize;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class PrizeService : IPrizeService
    {
        public Task<Result<PrizeResponse>> CreateAsync(CreatePrizeRequest createPrizeRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PrizeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PrizeResponse>> GetByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdatePrizeRequest updatePrizeRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}