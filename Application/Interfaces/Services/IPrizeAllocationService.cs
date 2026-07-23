using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.PrizeAllocation;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IPrizeAllocationService
    {
        public Task<Result<PrizeAllocationResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<PrizeAllocationResponse>>> GetAllByPrizeIdAsync(Guid prizeId, CancellationToken cancellationToken = default);
        public Task<Result<PrizeAllocationResponse>> CreateAsync(CreatePrizeAllocationRequest createPrizeAllocationRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdatePrizeAllocationRequest updatePrizeAllocationRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}