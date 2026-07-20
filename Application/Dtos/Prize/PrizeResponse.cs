using TournamentManager.Application.Dtos.PrizeAllocation;

namespace TournamentManager.Application.Dtos.Prize
{
    public class PrizeResponse
    {
        public Guid Id { get; set; }
        public decimal TotalPool { get; set; }
        public required string Currency { get; set; }
        public Guid TournamentId { get; set; }
        public ICollection<PrizeAllocationResponse> Allocations { get; set; } = new List<PrizeAllocationResponse>();
    }
}