using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TeamRepository : EFRepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public override async Task<Team?> GetAsync(Expression<Func<Team, bool>> predicate, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Team> query = Table.Include(t => t.Players);
            if (!enabledTracking) query = query.AsNoTracking();
            if (withDeleted) query = query.IgnoreQueryFilters();
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public override async Task<ICollection<Team>> GetAllAsync(Expression<Func<Team, bool>>? predicate = null, Func<IQueryable<Team>, IOrderedQueryable<Team>>? orderBy = null, bool withDeleted = false, bool enabledTracking = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Team> query = Table.Include(t => t.Players);
            if (!enabledTracking) query = query.AsNoTracking();
            if (withDeleted) query = query.IgnoreQueryFilters();
            if (predicate is not null) query = query.Where(predicate);
            if (orderBy is not null) query = orderBy(query);
            return await query.ToListAsync(cancellationToken);
        }
    }
}
