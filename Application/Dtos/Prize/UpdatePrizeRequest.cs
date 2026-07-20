using TournamentManager.Application.Dtos.PrizeAllocation;

namespace TournamentManager.Application.Dtos.Prize
{
    public class UpdatePrizeRequest
    {
        public decimal TotalPool { get; set; }
        public required string Currency { get; set; }
        public Guid TournamentId { get; set; }
        public ICollection<UpdatePrizeAllocationRequest> Allocations { get; set; } = new List<UpdatePrizeAllocationRequest>();
    }
}