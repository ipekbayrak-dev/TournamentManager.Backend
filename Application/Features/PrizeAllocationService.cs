using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.PrizeAllocation;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class PrizeAllocationService : IPrizeAllocationService
    {
        public Task<Result<PrizeAllocationResponse>> CreateAsync(CreatePrizeAllocationRequest createPrizeAllocationRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<PrizeAllocationResponse>>> GetAllByPrizeIdAsync(Guid prizeId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PrizeAllocationResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdatePrizeAllocationRequest updatePrizeAllocationRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}