using System.Linq.Expressions;
using TournamentManager.Application.Common;

namespace TournamentManager.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> GetAsync(
         Expression<Func<T, bool>> predicate,
         bool withDeleted = false,
         bool enabledTracking = false,
         CancellationToken cancellationToken = default
         );
        public Task<ICollection<T>> GetAllAsync(
         Expression<Func<T, bool>>? predicate = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
         bool withDeleted = false,
         bool enabledTracking = false,
         CancellationToken cancellationToken = default
         );
        public Task<bool> AnyAsync(
            Expression<Func<T, bool>>? predicate = null,
            bool withDeleted = false,
            bool enabledTracking = false,
            CancellationToken cancellationToken = default
        );
        public Task<Paginate<T>> GetPaginateAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enabledTracking = false,
            CancellationToken ct = default)
            ;
        Task<T> AddAsync(T entity);
        Task<ICollection<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<ICollection<T>> UpdateRangeAsync(IEnumerable<T> entities);
        Task<T> DeleteAsync(T entity);
        Task<ICollection<T>> DeleteRangeAsync(IEnumerable<T> entities);
    }
}