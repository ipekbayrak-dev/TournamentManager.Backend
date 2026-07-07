using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public IRepository<Match> Matches { get; }
        public IRepository<Payment> Payments { get; }
        public IRepository<Player> Players { get; }
        public IRepository<Prize> Prizes { get; }
        public IRepository<PrizeAllocation> PrizeAllocations { get; }
        public IRepository<Team> Teams { get; }
        public IRepository<Tournament> Tournaments { get; }
        public IRepository<TournamentEntry> TournamentEntries { get; }
        public Task SaveChangesAsync();
    }
}