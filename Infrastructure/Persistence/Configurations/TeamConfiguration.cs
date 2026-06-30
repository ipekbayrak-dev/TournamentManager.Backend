using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(b => b.Name)
                .HasMaxLength(63)
                .IsRequired();
            builder.Property(b => b.Handle)
                .HasMaxLength(15)
                .IsRequired();
            builder.HasIndex(b => b.Handle)
                .IsUnique();
            builder.Property(b => b.Logo)
                .IsRequired();
            builder.HasMany(b => b.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}