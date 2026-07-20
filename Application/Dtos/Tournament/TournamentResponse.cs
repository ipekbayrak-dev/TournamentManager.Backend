using TournamentManager.Application.Dtos.Match;
using TournamentManager.Application.Dtos.TournamentEntry;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Tournament
{
    public class TournamentResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; }
        public ICollection<MatchResponse> Matches { get; set; } = new List<MatchResponse>();
        public ICollection<TournamentEntryResponse> Entries { get; set; } = new List<TournamentEntryResponse>();
    }
}