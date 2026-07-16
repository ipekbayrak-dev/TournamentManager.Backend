using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class PrizeConfiguration : EntityBaseConfigurations<Prize>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Prize> builder)
        {
            builder.Property(b => b.TotalPool)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.Property(b => b.Currency)
                .HasMaxLength(3)
                .IsRequired();
            builder.HasMany(b => b.Allocations)
                .WithOne(a => a.Prize)
                .HasForeignKey(a => a.PrizeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}