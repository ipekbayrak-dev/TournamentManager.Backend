using TournamentManager.Domain.Entities;
using TournamentManager.Infrastructure.Persistence.Common;
using TournamentManager.Application.Interfaces.Repositories;

namespace TournamentManager.Infrastructure.Persistence.Repositories
{
    public class TournamentRepository : EFRepositoryBase<Tournament>, ITournamentRepository
    {
        public TournamentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}