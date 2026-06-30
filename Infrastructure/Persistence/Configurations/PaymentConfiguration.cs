using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(b => b.Amount)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.HasIndex(b => b.StripeSessionId)
                .IsUnique();
            builder.Property(b => b.StripeSessionId)
                .IsRequired();
            builder.HasIndex(b => b.StripePaymentIntentId)
                .IsUnique();
            builder.HasOne(b => b.TournamentEntry)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(b => b.TournamentEntryId);
            builder.Property(b => b.Currency)
                .HasDefaultValue("USD")
                .HasMaxLength(3);
        }
    }
}