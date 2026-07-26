using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Features
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }
        private static MatchResponse MapToResponse(Match match)
        {
            return new MatchResponse
            {
                Id = match.Id,
                BracketType = match.BracketType,
                RoundNumber = match.RoundNumber,
                TournamentId = match.TournamentId,
                TeamRadiantId = match.TeamRadiantId,
                TeamDireId = match.TeamDireId,
                TeamRadiantScore = match.TeamRadiantScore,
                TeamDireScore = match.TeamDireScore,
                Status = match.Status,
                ScheduledAt = match.ScheduledAt,
                CompletedAt = match.CompletedAt,
                WinnerTeamId = match.WinnerTeamId,
                WinnerAdvancesToMatchId = match.WinnerAdvancesToMatchId,
                LoserAdvancesToMatchId = match.LoserAdvancesToMatchId
            };
        }
        public async Task<Result<MatchResponse>> CreateAsync(CreateMatchRequest createMatchRequest, CancellationToken cancellationToken = default)
        {
            var match = new Match()
            {
                BracketType = createMatchRequest.BracketType,
                RoundNumber = createMatchRequest.RoundNumber,
                TournamentId = createMatchRequest.TournamentId,
                TeamRadiantId = createMatchRequest.TeamRadiantId,
                TeamDireId = createMatchRequest.TeamDireId,
                ScheduledAt = createMatchRequest.ScheduledAt,
                WinnerAdvancesToMatchId = createMatchRequest.WinnerAdvancesToMatchId,
                LoserAdvancesToMatchId = createMatchRequest.LoserAdvancesToMatchId,
                Status = MatchStatus.Scheduled
            };

            await _matchRepository.AddAsync(match);

            return Result<MatchResponse>.Success(MapToResponse(match));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure("Invalid match ID.");
            }

            var match = await _matchRepository.GetAsync(x => x.Id == id,cancellationToken: cancellationToken);

            if (match is null)
            {
                return Result.Failure("Match not found.");
            }

            await _matchRepository.DeleteAsync(match);

            return Result.Success();
        }

        public async Task<Result<ICollection<MatchResponse>>> GetAllByTournamentIdAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            if (tournamentId == Guid.Empty)
            {
                return Result<ICollection<MatchResponse>>.Failure("Invalid tournament ID.");
            }

            var matches = await _matchRepository.GetAllAsync(x => x.TournamentId == tournamentId,cancellationToken: cancellationToken);

            var response = matches.Select(MapToResponse).ToList();

            return Result<ICollection<MatchResponse>>.Success(response);
        }

        public async Task<Result<MatchResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                return Result<MatchResponse>.Failure("Invalid match ID.");
            }

            var match = await _matchRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (match is null)
            {
                return Result<MatchResponse>.Failure("Match not found.");
            }

            var response = MapToResponse(match);

            return Result<MatchResponse>.Success(response);
        }

        public async Task<Result> UpdateAsync(UpdateMatchRequest updateMatchRequest, CancellationToken cancellationToken = default)
        {
            if (updateMatchRequest.Id == Guid.Empty)
            {
                return Result.Failure("Invalid match ID.");
            }

            var match = await _matchRepository.GetAsync(x => x.Id == updateMatchRequest.Id, cancellationToken: cancellationToken);

            if (match is null)
            {
                return Result.Failure("Match not found.");
            }

            match.Status = updateMatchRequest.Status;
            match.TeamRadiantScore = updateMatchRequest.TeamRadiantScore;
            match.TeamDireScore = updateMatchRequest.TeamDireScore;
            match.CompletedAt = updateMatchRequest.CompletedAt;
            match.WinnerTeamId = updateMatchRequest.WinnerTeamId;

            await _matchRepository.UpdateAsync(match);

            return Result.Success();
        }
    }
}