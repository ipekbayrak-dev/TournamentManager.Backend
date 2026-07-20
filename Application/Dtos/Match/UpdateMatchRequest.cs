using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Match
{
    public class UpdateMatchRequest
    {
        public int TeamRadiantScore { get; set; }
        public int TeamDireScore { get; set; }
        public MatchStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? WinnerTeamId { get; set; }
    }
}