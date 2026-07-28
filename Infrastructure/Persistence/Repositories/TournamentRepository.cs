using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TournamentRepository : EFRepositoryBase<Tournament>, ITournamentRepository
    {
        public TournamentRepository(AppDbContext appDbContext) : base(appDbContext) { }

        protected override IQueryable<Tournament> ApplyIncludes(IQueryable<Tournament> query) =>
            query.Include(t => t.Matches).Include(t => t.Entries);
    }
}
