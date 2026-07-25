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
            builder.Ignore(x => x.IsActive);
            builder.HasQueryFilter(x => x.DeletedAt == null);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.DeletedAt);
            ConfigureEntity(builder);
        }
        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}