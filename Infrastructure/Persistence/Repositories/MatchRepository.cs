using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Application.Interfaces.Repositories;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class MatchRepository : EFRepositoryBase<Match>, IMatchRepository
    {
        public MatchRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}