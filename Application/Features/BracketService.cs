using TournamentManager.Application.Common;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Features
{
    public class BracketService : IBracketService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentEntryRepository _entryRepository;

        public BracketService(
            IMatchRepository matchRepository,
            ITournamentRepository tournamentRepository,
            ITournamentEntryRepository entryRepository)
        {
            _matchRepository = matchRepository;
            _tournamentRepository = tournamentRepository;
            _entryRepository = entryRepository;
        }

        public async Task<Result> GenerateBracketAsync(Guid tournamentId, CancellationToken cancellationToken = default)
        {
            if (tournamentId == Guid.Empty)
                return Result.Failure("Invalid tournament ID.");

            var tournament = await _tournamentRepository.GetAsync(
                x => x.Id == tournamentId, cancellationToken: cancellationToken);

            if (tournament is null)
                return Result.Failure("Tournament not found.");

            var existing = await _matchRepository.GetAllAsync(
                x => x.TournamentId == tournamentId, cancellationToken: cancellationToken);

            if (existing.Any())
                return Result.Failure("Bracket already exists for this tournament.");

            var entries = await _entryRepository.GetAllAsync(
                x => x.TournamentId == tournamentId && x.Status == EntryStatus.Approved,
                cancellationToken: cancellationToken);

            if (entries.Count != 8)
                return Result.Failure($"Exactly 8 approved entries are required to generate the bracket. Found: {entries.Count}.");

            var seeds = entries.OrderBy(e => e.Seed).ToList();
            var start = tournament.StartDate;

            // Pre-assign all 14 match IDs so cross-references can be set upfront
            var ubR1_1 = Guid.NewGuid(); // seed 1 vs seed 8
            var ubR1_2 = Guid.NewGuid(); // seed 4 vs seed 5
            var ubR1_3 = Guid.NewGuid(); // seed 2 vs seed 7
            var ubR1_4 = Guid.NewGuid(); // seed 3 vs seed 6
            var ubR2_1 = Guid.NewGuid(); // w(ubR1_1) vs w(ubR1_2)
            var ubR2_2 = Guid.NewGuid(); // w(ubR1_3) vs w(ubR1_4)
            var ubF    = Guid.NewGuid(); // w(ubR2_1) vs w(ubR2_2)  →  winner to GF
            var lbR1_1 = Guid.NewGuid(); // l(ubR1_1) vs l(ubR1_4)
            var lbR1_2 = Guid.NewGuid(); // l(ubR1_2) vs l(ubR1_3)
            var lbR2_1 = Guid.NewGuid(); // w(lbR1_1) vs l(ubR2_2)
            var lbR2_2 = Guid.NewGuid(); // w(lbR1_2) vs l(ubR2_1)
            var lbR3   = Guid.NewGuid(); // w(lbR2_1) vs w(lbR2_2)
            var lbF    = Guid.NewGuid(); // w(lbR3) vs l(ubF)
            var gf     = Guid.NewGuid(); // w(ubF) vs w(lbF)

            var matches = new List<Match>
            {
                // ── Upper Bracket R1 ──────────────────────────────────────────
                new() { Id = ubR1_1, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 1,
                        TeamRadiantId = seeds[0].TeamId, TeamDireId = seeds[7].TeamId,
                        ScheduledAt = start, Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = ubR2_1, LoserAdvancesToMatchId = lbR1_1 },

                new() { Id = ubR1_2, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 1,
                        TeamRadiantId = seeds[3].TeamId, TeamDireId = seeds[4].TeamId,
                        ScheduledAt = start.AddHours(3), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = ubR2_1, LoserAdvancesToMatchId = lbR1_2 },

                new() { Id = ubR1_3, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 1,
                        TeamRadiantId = seeds[1].TeamId, TeamDireId = seeds[6].TeamId,
                        ScheduledAt = start.AddHours(6), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = ubR2_2, LoserAdvancesToMatchId = lbR1_2 },

                new() { Id = ubR1_4, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 1,
                        TeamRadiantId = seeds[2].TeamId, TeamDireId = seeds[5].TeamId,
                        ScheduledAt = start.AddHours(9), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = ubR2_2, LoserAdvancesToMatchId = lbR1_1 },

                // ── Upper Bracket R2 ──────────────────────────────────────────
                new() { Id = ubR2_1, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 2,
                        ScheduledAt = start.AddDays(2), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = ubF, LoserAdvancesToMatchId = lbR2_2 },

                new() { Id = ubR2_2, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 2,
                        ScheduledAt = start.AddDays(2).AddHours(4), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = ubF, LoserAdvancesToMatchId = lbR2_1 },

                // ── Upper Bracket Final ───────────────────────────────────────
                new() { Id = ubF, TournamentId = tournamentId, BracketType = BracketType.Upper, RoundNumber = 3,
                        ScheduledAt = start.AddDays(4), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = gf, LoserAdvancesToMatchId = lbF },

                // ── Lower Bracket R1 ──────────────────────────────────────────
                new() { Id = lbR1_1, TournamentId = tournamentId, BracketType = BracketType.Lower, RoundNumber = 1,
                        ScheduledAt = start.AddDays(2).AddHours(8), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = lbR2_1 },

                new() { Id = lbR1_2, TournamentId = tournamentId, BracketType = BracketType.Lower, RoundNumber = 1,
                        ScheduledAt = start.AddDays(2).AddHours(12), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = lbR2_2 },

                // ── Lower Bracket R2 ──────────────────────────────────────────
                new() { Id = lbR2_1, TournamentId = tournamentId, BracketType = BracketType.Lower, RoundNumber = 2,
                        ScheduledAt = start.AddDays(4).AddHours(4), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = lbR3 },

                new() { Id = lbR2_2, TournamentId = tournamentId, BracketType = BracketType.Lower, RoundNumber = 2,
                        ScheduledAt = start.AddDays(4).AddHours(8), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = lbR3 },

                // ── Lower Bracket R3 ──────────────────────────────────────────
                new() { Id = lbR3, TournamentId = tournamentId, BracketType = BracketType.Lower, RoundNumber = 3,
                        ScheduledAt = start.AddDays(6), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = lbF },

                // ── Lower Bracket Final ───────────────────────────────────────
                new() { Id = lbF, TournamentId = tournamentId, BracketType = BracketType.Lower, RoundNumber = 4,
                        ScheduledAt = start.AddDays(8), Status = MatchStatus.Scheduled,
                        WinnerAdvancesToMatchId = gf },

                // ── Grand Final ───────────────────────────────────────────────
                new() { Id = gf, TournamentId = tournamentId, BracketType = BracketType.GrandFinal, RoundNumber = 1,
                        ScheduledAt = start.AddDays(10), Status = MatchStatus.Scheduled },
            };

            await _matchRepository.AddRangeAsync(matches);
            return Result.Success();
        }
    }
}
