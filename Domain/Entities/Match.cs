using TournamentManager.Domain.Common;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Domain.Entities
{
    public class Match : BaseEntity
    {
        public BracketType BracketType { get; set; }
        public int RoundNumber { get; set; }
        public Guid TournamentId { get; set; }
        public required Tournament Tournament { get; set; }
        public Guid? TeamRadiantId { get; set; }
        public Team? TeamRadiant { get; set; }
        public Guid? TeamDireId { get; set; }
        public Team? TeamDire { get; set; }
        public int TeamRadiantScore { get; set; }
        public int TeamDireScore { get; set; }
        public MatchStatus Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? WinnerTeamId { get; set; }

        public Guid? WinnerAdvancesToMatchId { get; set; }
        public Match? WinnerAdvancesToMatch { get; set; }
        public Guid? LoserAdvancesToMatchId { get; set; }
        public Match? LoserAdvancesToMatch { get; set; }
    }
}

