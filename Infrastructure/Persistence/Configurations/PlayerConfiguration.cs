using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(b => b.Handle)
                .IsRequired()
                .HasMaxLength(32);
            builder.Property(b => b.SteamId)
                .HasMaxLength(32);
            builder.HasIndex(b => b.SteamId)
                .IsUnique();
            builder.Property(b => b.FirstName)
                .IsRequired()
                .HasMaxLength(63);
            builder.Property(b => b.LastName)
                .IsRequired()
                .HasMaxLength(63);
            builder.Property(b => b.CountryCode)
                .IsRequired()
                .HasMaxLength(3);
            
        }
    }
}