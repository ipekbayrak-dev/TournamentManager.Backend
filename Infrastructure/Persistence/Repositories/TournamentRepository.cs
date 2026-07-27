using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TournamentRepository : EFRepositoryBase<Tournament>, ITournamentRepository
    {
        public TournamentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<Tournament?> GetAsync(Expression<Func<Tournament, bool>> predicate, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Tournament> query = Table.Include(t => t.Matches).Include(t => t.Entries);
            if (!enabledTracking) query = query.AsNoTracking();
            if (withDeleted) query = query.IgnoreQueryFilters();
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public override async Task<ICollection<Tournament>> GetAllAsync(Expression<Func<Tournament, bool>>? predicate = null, Func<IQueryable<Tournament>, IOrderedQueryable<Tournament>>? orderBy = null, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Tournament> query = Table.Include(t => t.Matches).Include(t => t.Entries);
            if (!enabledTracking) query = query.AsNoTracking();
            if (withDeleted) query = query.IgnoreQueryFilters();
            if (predicate is not null) query = query.Where(predicate);
            if (orderBy is not null) query = orderBy(query);
            return await query.ToListAsync(cancellationToken);
        }
    }
}
