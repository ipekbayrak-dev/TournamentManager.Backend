using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Prize;
using TournamentManager.Application.Dtos.PrizeAllocation;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Features
{
    public class PrizeService : IPrizeService
    {
        private readonly IPrizeRepository _prizeRepository;
        public PrizeService(IPrizeRepository prizeRepository)
        {
            _prizeRepository = prizeRepository;
        }
        private static PrizeResponse MapToResponse(Prize prize)
        {
            return new PrizeResponse
            {
                Id = prize.Id,
                TotalPool = prize.TotalPool,
                Currency = prize.Currency,
                TournamentId = prize.TournamentId,
                Allocations = prize.Allocations?.Select(x => new PrizeAllocationResponse
                {
                    Id = x.Id,
                    PrizeId = x.PrizeId,
                    Placement = x.Placement,
                    Percentage = x.Percentage,
                    WinningTeamId = x.WinningTeamId
                }).ToList() ?? new List<PrizeAllocationResponse>()
            };
        }

        public async Task<Result<PrizeResponse>> CreateAsync(CreatePrizeRequest createPrizeRequest, CancellationToken cancellationToken = default)
        {
            if (createPrizeRequest.TournamentId == Guid.Empty)
            {
                return Result<PrizeResponse>.Failure("Invalid tournament ID.");
            }

            var prize = new Prize
            {
                TotalPool = createPrizeRequest.TotalPool,
                Currency = createPrizeRequest.Currency,
                TournamentId = createPrizeRequest.TournamentId,
                Allocations = createPrizeRequest.Allocations.Select(x => new PrizeAllocation
                {
                    PrizeId = x.PrizeId,
                    Placement = x.Placement,
                    Percentage = x.Percentage
                }).ToList()
            };

            await _prizeRepository.AddAsync(prize);

            return Result<PrizeResponse>.Success(MapToResponse(prize));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var prize = await _prizeRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (prize is null)
            {
                return Result.Failure("Prize not found");
            }

            await _prizeRepository.DeleteAsync(prize);

            return Result.Success();
        }

        public async Task<Result<PrizeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var prize = await _prizeRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (prize is null)
            {
                return Result<PrizeResponse>.Failure("Prize not found.");
            }

            return Result<PrizeResponse>.Success(MapToResponse(prize));
        }

        public async Task<Result<PrizeResponse>> GetByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            var prize = await _prizeRepository.GetAsync(x => x.TournamentId == tournamentId, cancellationToken: cancellationToken);

            if (prize is null)
            {
                return Result<PrizeResponse>.Failure("Invalid Id");
            }

            return Result<PrizeResponse>.Success(MapToResponse(prize));
        }

        public async Task<Result> UpdateAsync(UpdatePrizeRequest updatePrizeRequest, CancellationToken cancellationToken = default)
        {
            var prize = await _prizeRepository.GetAsync(x => x.Id == updatePrizeRequest.Id, cancellationToken: cancellationToken);

            if (prize is null)
            {
                return Result.Failure("Invalid Id");
            }

            prize.TotalPool = updatePrizeRequest.TotalPool;
            prize.Currency = updatePrizeRequest.Currency;

            await _prizeRepository.UpdateAsync(prize);

            return Result.Success();
        }
    }
}