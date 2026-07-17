using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Persistence.Contracts
{
    public interface ITournamentEntryRepository : IRepository<TournamentEntry>
    {
        
    }
}