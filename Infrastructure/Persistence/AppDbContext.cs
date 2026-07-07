using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<PrizeAllocation> PrizeAllocations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentEntry> TournamentEntries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}