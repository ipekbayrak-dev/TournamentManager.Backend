using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.TournamentEntry
{
    public class UpdateTournamentEntryRequest
    {
        public int? Seed { get; set; }
        public EntryStatus Status { get; set; }
    }
}