using System.Linq.Expressions;
using Moq;
using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Features;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Enums;
using Xunit;
using Match = TournamentManager.Domain.Entities.Match;

namespace TournamentManager.Tests;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _repo = new();
    private readonly MatchService _sut;

    public MatchServiceTests()
    {
        _sut = new MatchService(_repo.Object);
    }

    private void SetupGet(Match? match)
        => _repo.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Match, bool>>>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

    private void SetupGetSequence(params Match?[] matches)
    {
        var seq = _repo.SetupSequence(r => r.GetAsync(
            It.IsAny<Expression<Func<Match, bool>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()));
        foreach (var m in matches)
            seq.ReturnsAsync(m);
    }

    // ── 1. Empty ID ──────────────────────────────────────────────────────────
    [Fact]
    public async Task UpdateAsync_ReturnsFailure_WhenMatchIdIsEmpty()
    {
        var result = await _sut.UpdateAsync(new UpdateMatchRequest { Id = Guid.Empty });

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid match ID.", result.ErrorMessage);
    }

    // ── 2. Match not found ───────────────────────────────────────────────────
    [Fact]
    public async Task UpdateAsync_ReturnsFailure_WhenMatchNotFound()
    {
        SetupGet(null);

        var result = await _sut.UpdateAsync(new UpdateMatchRequest { Id = Guid.NewGuid() });

        Assert.False(result.IsSuccess);
        Assert.Equal("Match not found.", result.ErrorMessage);
    }

    // ── 3. Returns success on valid update ───────────────────────────────────
    [Fact]
    public async Task UpdateAsync_ReturnsSuccess()
    {
        var match = new Match { Id = Guid.NewGuid(), Status = MatchStatus.Scheduled };
        SetupGet(match);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync(match);

        var result = await _sut.UpdateAsync(new UpdateMatchRequest { Id = match.Id });

        Assert.True(result.IsSuccess);
    }

    // ── 4. Properties updated ────────────────────────────────────────────────
    [Fact]
    public async Task UpdateAsync_UpdatesMatchProperties()
    {
        var match = new Match { Id = Guid.NewGuid(), Status = MatchStatus.Scheduled };
        SetupGet(match);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync(match);

        await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id,
            Status = MatchStatus.InProgress,
            TeamRadiantScore = 1,
            TeamDireScore = 0
        });

        Assert.Equal(MatchStatus.InProgress, match.Status);
        Assert.Equal(1, match.TeamRadiantScore);
        Assert.Equal(0, match.TeamDireScore);
    }

    // ── 5. Winner advances to empty Radiant slot ─────────────────────────────
    [Fact]
    public async Task UpdateAsync_AdvancesWinner_ToRadiantSlot_OnFirstCompletion()
    {
        var teamA = Guid.NewGuid();
        var teamB = Guid.NewGuid();
        var nextId = Guid.NewGuid();

        var match = new Match
        {
            Id = Guid.NewGuid(), Status = MatchStatus.Scheduled,
            TeamRadiantId = teamA, TeamDireId = teamB,
            WinnerAdvancesToMatchId = nextId
        };
        var next = new Match { Id = nextId, TeamRadiantId = null, TeamDireId = null };

        SetupGetSequence(match, next);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync((Match m) => m);

        await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id, Status = MatchStatus.Completed, WinnerTeamId = teamA
        });

        Assert.Equal(teamA, next.TeamRadiantId);
    }

    // ── 6. Winner fills Dire slot when Radiant already taken ─────────────────
    [Fact]
    public async Task UpdateAsync_AdvancesWinner_ToDireSlot_WhenRadiantAlreadyFilled()
    {
        var teamA = Guid.NewGuid();
        var teamB = Guid.NewGuid();
        var existing = Guid.NewGuid();
        var nextId = Guid.NewGuid();

        var match = new Match
        {
            Id = Guid.NewGuid(), Status = MatchStatus.Scheduled,
            TeamRadiantId = teamA, TeamDireId = teamB,
            WinnerAdvancesToMatchId = nextId
        };
        var next = new Match { Id = nextId, TeamRadiantId = existing, TeamDireId = null };

        SetupGetSequence(match, next);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync((Match m) => m);

        await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id, Status = MatchStatus.Completed, WinnerTeamId = teamA
        });

        Assert.Equal(existing, next.TeamRadiantId); // untouched
        Assert.Equal(teamA, next.TeamDireId);
    }

    // ── 7. Loser advances to next match ──────────────────────────────────────
    [Fact]
    public async Task UpdateAsync_AdvancesLoser_ToNextMatch_OnFirstCompletion()
    {
        var teamA = Guid.NewGuid();
        var teamB = Guid.NewGuid();
        var loserNextId = Guid.NewGuid();

        var match = new Match
        {
            Id = Guid.NewGuid(), Status = MatchStatus.Scheduled,
            TeamRadiantId = teamA, TeamDireId = teamB,
            LoserAdvancesToMatchId = loserNextId
        };
        var loserNext = new Match { Id = loserNextId, TeamRadiantId = null };

        SetupGetSequence(match, loserNext);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync((Match m) => m);

        await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id, Status = MatchStatus.Completed, WinnerTeamId = teamA
        });

        Assert.Equal(teamB, loserNext.TeamRadiantId); // teamB lost
    }

    // ── 8. No advance when match was already completed ────────────────────────
    [Fact]
    public async Task UpdateAsync_DoesNotAdvance_WhenMatchAlreadyCompleted()
    {
        var teamA = Guid.NewGuid();
        var match = new Match
        {
            Id = Guid.NewGuid(), Status = MatchStatus.Completed, // already done
            TeamRadiantId = teamA,
            WinnerAdvancesToMatchId = Guid.NewGuid()
        };
        SetupGet(match);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync(match);

        await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id, Status = MatchStatus.Completed, WinnerTeamId = teamA
        });

        _repo.Verify(r => r.GetAsync(
            It.IsAny<Expression<Func<Match, bool>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once); // only the initial lookup, no advance lookups
    }

    // ── 9. No advance when WinnerTeamId is null ───────────────────────────────
    [Fact]
    public async Task UpdateAsync_DoesNotAdvance_WhenWinnerTeamIdNotSet()
    {
        var match = new Match
        {
            Id = Guid.NewGuid(), Status = MatchStatus.Scheduled,
            WinnerAdvancesToMatchId = Guid.NewGuid()
        };
        SetupGet(match);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync(match);

        await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id, Status = MatchStatus.Completed, WinnerTeamId = null
        });

        _repo.Verify(r => r.GetAsync(
            It.IsAny<Expression<Func<Match, bool>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ── 10. No advance when both next-match IDs are null ─────────────────────
    [Fact]
    public async Task UpdateAsync_DoesNotAdvance_WhenNoNextMatchIds()
    {
        var teamA = Guid.NewGuid();
        var match = new Match
        {
            Id = Guid.NewGuid(), Status = MatchStatus.Scheduled,
            TeamRadiantId = teamA,
            WinnerAdvancesToMatchId = null,
            LoserAdvancesToMatchId = null
        };
        SetupGet(match);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Match>())).ReturnsAsync(match);

        var result = await _sut.UpdateAsync(new UpdateMatchRequest
        {
            Id = match.Id, Status = MatchStatus.Completed, WinnerTeamId = teamA
        });

        Assert.True(result.IsSuccess);
        _repo.Verify(r => r.GetAsync(
            It.IsAny<Expression<Func<Match, bool>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
