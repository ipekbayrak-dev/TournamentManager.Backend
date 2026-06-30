
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasOne(b => b.TeamDire)
                .WithMany()
                .HasForeignKey(b => b.TeamDireId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.TeamRadiant)
                .WithMany()
                .HasForeignKey(b => b.TeamRadiantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.LoserAdvancesToMatch)
                .WithMany()
                .HasForeignKey(b => b.LoserAdvancesToMatchId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.WinnerAdvancesToMatch)
                .WithMany()
                .HasForeignKey(b => b.WinnerAdvancesToMatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}