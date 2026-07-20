using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Dtos.TournamentEntry;

namespace TournamentManager.Application.Dtos.Tournament
{
    public class CreateTournamentRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<CreateTournamentEntryRequest> Entries { get; set; } = new List<CreateTournamentEntryRequest>();
    }
}