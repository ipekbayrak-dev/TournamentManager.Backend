using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Dtos.TournamentEntry;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Tournament
{
    public class UpdateTournamentRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; }
        public ICollection<UpdateMatchRequest> Matches { get; set; } = new List<UpdateMatchRequest>();
        public ICollection<UpdateTournamentEntryRequest> Entries { get; set; } = new List<UpdateTournamentEntryRequest>();
    }
}