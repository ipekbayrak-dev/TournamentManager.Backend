using System.Linq.Expressions;

namespace TournamentManager.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<TEntity?> GetAsync(
         Expression<Func<TEntity, bool>> predicate,
         bool withDeleted = false,
         bool enabledTracking = false,
         CancellationToken cancellationToken = default
         );
    }
}