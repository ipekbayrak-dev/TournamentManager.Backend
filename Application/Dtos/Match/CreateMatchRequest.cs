using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Match
{
    public class CreateMatchRequest
    {
        public BracketType BracketType { get; set; }
        public int RoundNumber { get; set; }
        public Guid TournamentId { get; set; }
        public Guid? TeamRadiantId { get; set; }
        public Guid? TeamDireId { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}