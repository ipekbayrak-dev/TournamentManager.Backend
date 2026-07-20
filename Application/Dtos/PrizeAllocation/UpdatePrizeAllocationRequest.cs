namespace TournamentManager.Application.Dtos.PrizeAllocation
{
    public class UpdatePrizeAllocationRequest
    {
        public Guid PrizeId { get; set; }
        public int Placement { get; set; }
        public decimal Percentage { get; set; }
    }
}