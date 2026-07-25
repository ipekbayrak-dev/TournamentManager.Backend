using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Application.Interfaces.Repositories;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TournamentEntryRepository : EFRepositoryBase<TournamentEntry>, ITournamentEntryRepository
    {
        public TournamentEntryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }
    }
}