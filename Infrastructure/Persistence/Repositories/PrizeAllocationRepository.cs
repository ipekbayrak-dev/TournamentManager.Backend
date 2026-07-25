using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Application.Interfaces.Repositories;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class PrizeAllocationRepository : EFRepositoryBase<PrizeAllocation>, IPrizeAllocationRepository
    {
        public PrizeAllocationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }
    }
}