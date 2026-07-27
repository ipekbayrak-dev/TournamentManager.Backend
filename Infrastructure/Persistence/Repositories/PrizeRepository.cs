using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class PrizeRepository : EFRepositoryBase<Prize>, IPrizeRepository
    {
        public PrizeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<Prize?> GetAsync(Expression<Func<Prize, bool>> predicate, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Prize> query = Table.Include(p => p.Allocations);
            if (!enabledTracking) query = query.AsNoTracking();
            if (withDeleted) query = query.IgnoreQueryFilters();
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public override async Task<ICollection<Prize>> GetAllAsync(Expression<Func<Prize, bool>>? predicate = null, Func<IQueryable<Prize>, IOrderedQueryable<Prize>>? orderBy = null, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Prize> query = Table.Include(p => p.Allocations);
            if (!enabledTracking) query = query.AsNoTracking();
            if (withDeleted) query = query.IgnoreQueryFilters();
            if (predicate is not null) query = query.Where(predicate);
            if (orderBy is not null) query = orderBy(query);
            return await query.ToListAsync(cancellationToken);
        }
    }
}
