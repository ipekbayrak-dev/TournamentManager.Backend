using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Infrastructure.Persistence.Contracts;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TeamRepository : EFRepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}