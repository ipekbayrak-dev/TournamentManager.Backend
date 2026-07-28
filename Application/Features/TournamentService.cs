using FluentValidation;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Dtos.Tournament;
using TournamentManager.Application.Dtos.TournamentEntry;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Features
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IValidator<CreateTournamentRequest> _createValidator;
        public TournamentService(ITournamentRepository tournamentRepository, IValidator<CreateTournamentRequest> createValidator)
        {
            _tournamentRepository = tournamentRepository;
            _createValidator = createValidator;
        }
        private static TournamentResponse MapToResponse(Tournament tournament)
        {
            return new TournamentResponse
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Location = tournament.Location,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                Status = tournament.Status,
                Matches = tournament.Matches.Select(x => new MatchResponse
                {
                    Id = x.Id,
                    BracketType = x.BracketType,
                    RoundNumber = x.RoundNumber,
                    TournamentId = x.TournamentId,
                    TeamRadiantId = x.TeamRadiantId,
                    TeamDireId = x.TeamDireId,
                    TeamRadiantScore = x.TeamRadiantScore,
                    TeamDireScore = x.TeamDireScore,
                    Status = x.Status,
                    ScheduledAt = x.ScheduledAt,
                    CompletedAt = x.CompletedAt,
                    WinnerTeamId = x.WinnerTeamId,
                    WinnerAdvancesToMatchId = x.WinnerAdvancesToMatchId,
                    LoserAdvancesToMatchId = x.LoserAdvancesToMatchId
                }).ToList(),

                Entries = tournament.Entries.Select(x => new TournamentEntryResponse
                {
                    Id = x.Id,
                    TournamentId = x.TournamentId,
                    RegisteredAt = x.RegisteredAt,
                    Seed = x.Seed,
                    Status = x.Status,
                    TeamId = x.TeamId
                }).ToList()
            };
        }
        public async Task<Result<TournamentResponse>> CreateAsync(CreateTournamentRequest createTournamentRequest, CancellationToken cancellationToken = default)
        {
            var validation = await _createValidator.ValidateAsync(createTournamentRequest, cancellationToken);

            if (!validation.IsValid)
                return Result<TournamentResponse>.Failure(validation.ToErrorMessage());

            var tournament = new Tournament
            {
                Name = createTournamentRequest.Name,
                Description = createTournamentRequest.Description,
                Location = createTournamentRequest.Location,
                StartDate = createTournamentRequest.StartDate,
                EndDate = createTournamentRequest.EndDate,
                Status = TournamentStatus.Upcoming
            };

            await _tournamentRepository.AddAsync(tournament);
            return Result<TournamentResponse>.Success(MapToResponse(tournament));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tournament = await _tournamentRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (tournament is null)
            {
                return Result.Failure("Tournament not found");
            }

            await _tournamentRepository.DeleteAsync(tournament);

            return Result.Success();
        }

        public async Task<Result<ICollection<TournamentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var tournament = await _tournamentRepository.GetAllAsync(cancellationToken: cancellationToken);

            var response = tournament.Select(MapToResponse).ToList();

            return Result<ICollection<TournamentResponse>>.Success(response);
        }

        public async Task<Result<TournamentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tournament = await _tournamentRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (tournament is null)
            {
                return Result<TournamentResponse>.Success(null);
            }

            return Result<TournamentResponse>.Success(MapToResponse(tournament));
        }

        public async Task<Result> UpdateAsync(UpdateTournamentRequest updateTournamentRequest, CancellationToken cancellationToken = default)
        {
            var tournament = await _tournamentRepository.GetAsync(x => x.Id == updateTournamentRequest.Id, cancellationToken: cancellationToken);

            if (tournament is null)
            {
                return Result.Failure("Invalid Id");
            }

            tournament.Name = updateTournamentRequest.Name;
            tournament.Description = updateTournamentRequest.Description;
            tournament.Location = updateTournamentRequest.Location;
            tournament.StartDate = updateTournamentRequest.StartDate;
            tournament.EndDate = updateTournamentRequest.EndDate;
            tournament.Status = updateTournamentRequest.Status;

            await _tournamentRepository.UpdateAsync(tournament);

            return Result.Success();
        }
    }
}