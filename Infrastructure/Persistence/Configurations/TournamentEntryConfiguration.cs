using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class TournamentEntryConfiguration : EntityBaseConfigurations<TournamentEntry>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TournamentEntry> builder)
        {
            builder.HasIndex(b => new { b.TeamId, b.TournamentId })
                .IsUnique();
            builder.Property(b => b.RegisteredAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}