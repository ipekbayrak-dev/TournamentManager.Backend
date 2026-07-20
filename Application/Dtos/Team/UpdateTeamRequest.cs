using TournamentManager.Application.Dtos.Player;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Team
{
    public class UpdateTeamRequest
    {
        public required string Name { get; set; }
        public required string Handle { get; set; }
        public required string Logo { get; set; }
        public DotaRegion Region { get; set; }
        public ICollection<UpdatePlayerRequest> Players { get; set; } = new List<UpdatePlayerRequest>();
    }
}