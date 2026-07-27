using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Common;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Common;
using TournamentManager.Infrastructure.Common;

namespace TournamentManager.Infrastructure.Persistence.Common
{
    public class EFRepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _appDbContext;
        protected DbSet<T> Table => _appDbContext.Set<T>();
        public EFRepositoryBase(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = Table;
            if (!enabledTracking)
                query = query.AsNoTracking();
            if (withDeleted)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = Table;
            if (!enabledTracking)
                query = query.AsNoTracking();
            if (withDeleted)
                query = query.IgnoreQueryFilters();
            if (predicate is not null)
                query = query.Where(predicate);
            if (orderBy is not null)
                query = orderBy(query);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = Table;
            if (!enabledTracking)
                query = query.AsNoTracking();
            if (withDeleted)
                query = query.IgnoreQueryFilters();
            if (predicate is not null)
                query = query.Where(predicate);
            return await query.AnyAsync(cancellationToken);
        }

        public Task<Paginate<T>> GetPaginateAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int index = 0, int size = 10, bool withDeleted = false, bool enabledTracking = false, CancellationToken ct = default)
        {
            IQueryable<T> query = Table;
            if (!enabledTracking)
                query = query.AsNoTracking();
            if (withDeleted)
                query = query.IgnoreQueryFilters();
            if (predicate is not null)
                query = query.Where(predicate);
            if (orderBy is not null)
                query = orderBy(query);
            return query.ToPaginateAsync(index, size, ct);
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedAt = DateTimeOffset.UtcNow;
            await Table.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.CreatedAt = DateTimeOffset.UtcNow;
            await Table.AddRangeAsync(entities);
            await _appDbContext.SaveChangesAsync();
            return entities.ToList();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            Table.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.UpdatedAt = DateTimeOffset.UtcNow;
            Table.UpdateRange(entities);
            await _appDbContext.SaveChangesAsync();
            return entities.ToList();
        }

        public async Task<T> DeleteAsync(T entity)
        {
            entity.DeletedAt = DateTimeOffset.UtcNow;
            Table.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<T>> DeleteRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.DeletedAt = DateTimeOffset.UtcNow;
            Table.UpdateRange(entities);
            await _appDbContext.SaveChangesAsync();
            return entities.ToList();
        }
    }
}