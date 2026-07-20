namespace TournamentManager.Application.Dtos.PrizeAllocation
{
    public class PrizeAllocationResponse
    {
        public Guid Id { get; set; }
        public Guid PrizeId { get; set; }
        public int Placement { get; set; }
        public decimal Percentage { get; set; }
        public Guid? WinningTeamId { get; set; }
    }
}