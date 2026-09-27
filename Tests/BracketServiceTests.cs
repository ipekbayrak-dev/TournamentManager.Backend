using System.Linq.Expressions;
using Moq;
using TournamentManager.Application.Features;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Enums;
using Xunit;
using Match = TournamentManager.Domain.Entities.Match;
using Tournament = TournamentManager.Domain.Entities.Tournament;
using TournamentEntry = TournamentManager.Domain.Entities.TournamentEntry;

namespace TournamentManager.Tests;

public class BracketServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepo = new();
    private readonly Mock<ITournamentRepository> _tournamentRepo = new();
    private readonly Mock<ITournamentEntryRepository> _entryRepo = new();
    private readonly BracketService _sut;

    public BracketServiceTests()
    {
        _sut = new BracketService(_matchRepo.Object, _tournamentRepo.Object, _entryRepo.Object);
    }

    private void SetupTournament(Tournament? t)
        => _tournamentRepo.Setup(r => r.GetAsync(
            It.IsAny<Expression<Func<Tournament, bool>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(t);

    private void SetupMatches(ICollection<Match> matches)
        => _matchRepo.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<Match, bool>>>(),
            It.IsAny<Func<IQueryable<Match>, IOrderedQueryable<Match>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

    private void SetupEntries(ICollection<TournamentEntry> entries)
        => _entryRepo.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<TournamentEntry, bool>>>(),
            It.IsAny<Func<IQueryable<TournamentEntry>, IOrderedQueryable<TournamentEntry>>>(),
            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entries);

    private static List<TournamentEntry> MakeEntries(int count) =>
        Enumerable.Range(1, count)
            .Select(i => new TournamentEntry { TeamId = Guid.NewGuid(), Seed = i, Status = EntryStatus.Approved })
            .ToList();

    // ── 1. Empty ID ──────────────────────────────────────────────────────────
    [Fact]
    public async Task GenerateBracketAsync_ReturnsFailure_WhenTournamentIdIsEmpty()
    {
        var result = await _sut.GenerateBracketAsync(Guid.Empty);

        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid tournament ID.", result.ErrorMessage);
    }

    // ── 2. Tournament not found ───────────────────────────────────────────────
    [Fact]
    public async Task GenerateBracketAsync_ReturnsFailure_WhenTournamentNotFound()
    {
        SetupTournament(null);

        var result = await _sut.GenerateBracketAsync(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Equal("Tournament not found.", result.ErrorMessage);
    }

    // ── 3. Bracket already exists ─────────────────────────────────────────────
    [Fact]
    public async Task GenerateBracketAsync_ReturnsFailure_WhenBracketAlreadyExists()
    {
        SetupTournament(new Tournament { Id = Guid.NewGuid(), Name = "Test", StartDate = DateTime.UtcNow });
        SetupMatches(new List<Match> { new() });

        var result = await _sut.GenerateBracketAsync(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Equal("Bracket already exists for this tournament.", result.ErrorMessage);
    }

    // ── 4. Fewer than 8 approved entries ──────────────────────────────────────
    [Fact]
    public async Task GenerateBracketAsync_ReturnsFailure_WhenFewerThanEightApprovedEntries()
    {
        SetupTournament(new Tournament { Id = Guid.NewGuid(), Name = "Test", StartDate = DateTime.UtcNow });
        SetupMatches(new List<Match>());
        SetupEntries(MakeEntries(7));

        var result = await _sut.GenerateBracketAsync(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Contains("7", result.ErrorMessage);
    }

    // ── 5. Exactly 8 entries → success + 14 matches created ──────────────────
    [Fact]
    public async Task GenerateBracketAsync_ReturnsSuccess_AndCreates14Matches_WhenEightApprovedEntries()
    {
        SetupTournament(new Tournament { Id = Guid.NewGuid(), Name = "Test", StartDate = DateTime.UtcNow });
        SetupMatches(new List<Match>());
        SetupEntries(MakeEntries(8));
        _matchRepo.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Match>>()))
            .ReturnsAsync(new List<Match>());

        var result = await _sut.GenerateBracketAsync(Guid.NewGuid());

        Assert.True(result.IsSuccess);
        _matchRepo.Verify(r => r.AddRangeAsync(
            It.Is<IEnumerable<Match>>(m => m.Count() == 14)), Times.Once);
    }
}
