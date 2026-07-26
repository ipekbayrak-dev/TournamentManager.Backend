using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.TournamentEntry;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Features
{
    public class TournamentEntryService : ITournamentEntryService
    {
        private readonly ITournamentEntryRepository _tournamentEntryRepository;
        public TournamentEntryService(ITournamentEntryRepository tournamentEntryRepository)
        {
            _tournamentEntryRepository = tournamentEntryRepository;
        }
        private static TournamentEntryResponse MapToResponse(TournamentEntry tournamentEntry)
        {
            return new TournamentEntryResponse
            {
                Id = tournamentEntry.Id,
                TournamentId = tournamentEntry.TournamentId,
                TeamId = tournamentEntry.TeamId,
                RegisteredAt = tournamentEntry.RegisteredAt,
                Seed = tournamentEntry.Seed,
                Status = tournamentEntry.Status
            };
        }
        public async Task<Result<TournamentEntryResponse>> CreateAsync(CreateTournamentEntryRequest createTournamentEntryRequest, CancellationToken cancellationToken = default)
        {
            if (createTournamentEntryRequest.TournamentId == Guid.Empty)
            {
                return Result<TournamentEntryResponse>.Failure("Invalid tournament ID.");
            }

            var tournamentEntry = new TournamentEntry
            {
                TournamentId = createTournamentEntryRequest.TournamentId,
                TeamId = createTournamentEntryRequest.TeamId,
                Seed = createTournamentEntryRequest.Seed
            };

            await _tournamentEntryRepository.AddAsync(tournamentEntry);
            return Result<TournamentEntryResponse>.Success(MapToResponse(tournamentEntry));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tournamentEntry = await _tournamentEntryRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (tournamentEntry is null)
            {
                return Result.Failure("Tournament entry not found");
            }

            await _tournamentEntryRepository.DeleteAsync(tournamentEntry);

            return Result.Success();
        }

        public async Task<Result<ICollection<TournamentEntryResponse>>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            var tournamentEntry = await _tournamentEntryRepository.GetAllAsync(
                x => x.TournamentId == tournamentId,
                cancellationToken: cancellationToken
            );

            var response = tournamentEntry.Select(MapToResponse).ToList();

            return Result<ICollection<TournamentEntryResponse>>.Success(response);
        }

        public async Task<Result<TournamentEntryResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tournamentEntry = await _tournamentEntryRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
            if (tournamentEntry is null)
            {
                return Result<TournamentEntryResponse>.Failure("Invalid Id");
            }

            return Result<TournamentEntryResponse>.Success(MapToResponse(tournamentEntry));
        }

        public async Task<Result> UpdateAsync(UpdateTournamentEntryRequest updateTournamentEntryRequest, CancellationToken cancellationToken = default)
        {
            var tournamentEntry = await _tournamentEntryRepository.GetAsync(x => x.Id == updateTournamentEntryRequest.Id, cancellationToken: cancellationToken);

            if (tournamentEntry is null)
            {
                return Result.Failure("Invalid Id");
            }

            tournamentEntry.Seed = updateTournamentEntryRequest.Seed;
            tournamentEntry.Status = updateTournamentEntryRequest.Status;

            await _tournamentEntryRepository.UpdateAsync(tournamentEntry);

            return Result.Success();
        }
    }
}