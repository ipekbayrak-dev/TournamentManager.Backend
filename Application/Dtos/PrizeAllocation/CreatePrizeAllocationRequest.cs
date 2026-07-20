namespace TournamentManager.Application.Dtos.PrizeAllocation
{
    public class CreatePrizeAllocationRequest
    {
        public Guid PrizeId { get; set; }
        public int Placement { get; set; }
        public decimal Percentage { get; set; }
    }
}