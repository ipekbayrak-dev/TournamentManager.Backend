using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class PrizeAllocationConfiguration : IEntityTypeConfiguration<PrizeAllocation>
    {
        public void Configure(EntityTypeBuilder<PrizeAllocation> builder)
        {
            builder.Property(b => b.Percentage)
                .HasPrecision(5, 2)
                .IsRequired();
            builder.HasOne(b => b.WinningTeam)
                .WithMany()
                .HasForeignKey(b => b.WinningTeamId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Ignore(b => b.CalculatedPayoutAmount);
        }
    }
}