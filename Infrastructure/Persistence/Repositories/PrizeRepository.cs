using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class PrizeRepository : EFRepositoryBase<Prize>, IPrizeRepository
    {
        public PrizeRepository(AppDbContext appDbContext) : base(appDbContext) { }

        protected override IQueryable<Prize> ApplyIncludes(IQueryable<Prize> query) =>
            query.Include(p => p.Allocations);
    }
}
