using TournamentManager.Application.Dtos.PrizeAllocation;

namespace TournamentManager.Application.Dtos.Prize
{
    public class CreatePrizeRequest
    {
        public decimal TotalPool { get; set; }
        public required string Currency { get; set; }
        public Guid TournamentId { get; set; }
        public ICollection<CreatePrizeAllocationRequest> Allocations { get; set; } = new List<CreatePrizeAllocationRequest>();
    }
}