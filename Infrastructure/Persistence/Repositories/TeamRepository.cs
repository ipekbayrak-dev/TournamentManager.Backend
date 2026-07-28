using Microsoft.EntityFrameworkCore;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TeamRepository : EFRepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext appDbContext) : base(appDbContext) { }

        protected override IQueryable<Team> ApplyIncludes(IQueryable<Team> query) =>
            query.Include(t => t.Players);
    }
}
