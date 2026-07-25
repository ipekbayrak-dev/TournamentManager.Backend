using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Application.Interfaces.Repositories;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class PlayerRepository : EFRepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }
    }
}