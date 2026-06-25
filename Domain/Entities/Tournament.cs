using TournamentManager.Domain.Common;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; } = "Online";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; }
        public Prize? Prize { get; set; } 
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public ICollection<TournamentEntry> Entries { get; set; } = new List<TournamentEntry>();
    }
}