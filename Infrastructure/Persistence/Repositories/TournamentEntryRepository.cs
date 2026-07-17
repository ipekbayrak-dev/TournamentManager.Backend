using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Infrastructure.Persistence.Contracts;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TournamentEntryRepository : EFRepositoryBase<TournamentEntry>, ITournamentEntryRepository
    {
        public TournamentEntryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }
    }
}