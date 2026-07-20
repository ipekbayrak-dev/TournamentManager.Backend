using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.TournamentEntry
{
    public class CreateTournamentEntryRequest
    {
        public Guid TournamentId { get; set; }
        public Guid TeamId { get; set; }
        public int? Seed { get; set; }
    }
}