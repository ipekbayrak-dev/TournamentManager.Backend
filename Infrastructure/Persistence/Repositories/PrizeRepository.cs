using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Infrastructure.Persistence.Contracts;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class PrizeRepository : EFRepositoryBase<Prize>, IPrizeRepository
    {
        public PrizeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }
    }
}