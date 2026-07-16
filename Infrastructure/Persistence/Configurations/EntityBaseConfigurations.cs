using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentManager.Domain.Common;

namespace TournamentManager.Infrastructure.Persistence.Configurations
{
    public abstract class EntityBaseConfigurations<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedAt)
                .IsRequired();
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);
            builder.Property(x => x.DeletedAt)
                .IsRequired(false);
            builder.Property(x => x.IsActive)
                .IsRequired();
            builder.HasQueryFilter(x => x.IsActive);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.IsActive)
                .HasFilter("[IsActive] = 1");
            ConfigureEntity(builder);
        }
        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}