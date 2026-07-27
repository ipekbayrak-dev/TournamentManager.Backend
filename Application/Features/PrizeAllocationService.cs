using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.PrizeAllocation;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Features
{
    public class PrizeAllocationService : IPrizeAllocationService
    {
        private readonly IPrizeAllocationRepository _prizeAllocationRepository;
        public PrizeAllocationService(IPrizeAllocationRepository prizeAllocationRepository)
        {
            _prizeAllocationRepository = prizeAllocationRepository;
        }
        private static PrizeAllocationResponse MapToResponse(PrizeAllocation prizeAllocation)
        {
            return new PrizeAllocationResponse
            {
                Id = prizeAllocation.Id,
                PrizeId = prizeAllocation.PrizeId,
                Placement = prizeAllocation.Placement,
                Percentage = prizeAllocation.Percentage,
                WinningTeamId = prizeAllocation.WinningTeamId
            };
        }
        public async Task<Result<PrizeAllocationResponse>> CreateAsync(CreatePrizeAllocationRequest createPrizeAllocationRequest, CancellationToken cancellationToken = default)
        {
            if (createPrizeAllocationRequest.PrizeId == Guid.Empty)
            {
                return Result<PrizeAllocationResponse>.Failure("Invalid prize ID.");
            }

            var prizeAllocation = new PrizeAllocation
            {
                PrizeId = createPrizeAllocationRequest.PrizeId,
                Placement = createPrizeAllocationRequest.Placement,
                Percentage = createPrizeAllocationRequest.Percentage
            };

            await _prizeAllocationRepository.AddAsync(prizeAllocation);

            return Result<PrizeAllocationResponse>.Success(MapToResponse(prizeAllocation));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var prizeAllocation = await _prizeAllocationRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (prizeAllocation is null)
            {
                return Result.Failure("Allocation not found");
            }

            await _prizeAllocationRepository.DeleteAsync(prizeAllocation);

            return Result.Success();
        }

        public async Task<Result<ICollection<PrizeAllocationResponse>>> GetAllByPrizeIdAsync(Guid prizeId, CancellationToken cancellationToken = default)
        {
            var prizeAllocation = await _prizeAllocationRepository.GetAllAsync(x => x.PrizeId == prizeId, cancellationToken: cancellationToken);

            var response = prizeAllocation.Select(MapToResponse).ToList();

            return Result<ICollection<PrizeAllocationResponse>>.Success(response);
        }

        public async Task<Result<PrizeAllocationResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var prizeAllocation = await _prizeAllocationRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (prizeAllocation is null)
            {
                return Result<PrizeAllocationResponse>.Success(null);
            }

            return Result<PrizeAllocationResponse>.Success(MapToResponse(prizeAllocation));
        }

        public async Task<Result> UpdateAsync(UpdatePrizeAllocationRequest updatePrizeAllocationRequest, CancellationToken cancellationToken = default)
        {
            var prizeAllocation = await _prizeAllocationRepository.GetAsync(x => x.Id == updatePrizeAllocationRequest.Id, cancellationToken: cancellationToken);

            if (prizeAllocation is null)
            {
                return Result.Failure("Invalid Id");
            }

            prizeAllocation.Placement = updatePrizeAllocationRequest.Placement;
            prizeAllocation.Percentage = updatePrizeAllocationRequest.Percentage;

            await _prizeAllocationRepository.UpdateAsync(prizeAllocation);

            return Result.Success();
        }
    }
}