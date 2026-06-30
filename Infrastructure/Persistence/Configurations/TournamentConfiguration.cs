using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.Property(b => b.Name)
                .HasMaxLength(63)
                .IsRequired();
            builder.Property(b => b.Description)
                .HasMaxLength(255);
            builder.Property(b => b.Location)
                .HasMaxLength(15);
            builder.HasMany(b => b.Matches)
                .WithOne(b => b.Tournament)
                .HasForeignKey(b => b.TournamentId);
            builder.HasMany(b => b.Entries)
                .WithOne(b => b.Tournament)
                .HasForeignKey(b => b.TournamentId);
            builder.HasOne(b => b.Prize)
                .WithOne(b => b.Tournament)
                .HasForeignKey<Prize>(b => b.TournamentId);
        }
    }
}