using TournamentManager.Domain.Common;

namespace TournamentManager.Domain.Entities
{
    public class Prize : BaseEntity
    {
        public required decimal TotalPool { get; set; }
        public string Currency { get; set; } = "USD";
        public Guid TournamentId { get; set; }
        public required Tournament Tournament { get; set; }
        public ICollection<PrizeAllocation> Allocations { get; set; } = new List<PrizeAllocation>();
    }
}