using TournamentManager.Domain.Common;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Domain.Entities
{
    public class Team : BaseEntity
    {
        public required string Name { get; set; }
        public required string Handle { get; set; }
        public required string Logo { get; set; }
        public DotaRegion Region { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}