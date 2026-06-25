using TournamentManager.Domain.Common;

namespace TournamentManager.Domain.Entities;

public class PrizeAllocation : BaseEntity
{
    public Guid PrizeId { get; set; }
    public required Prize Prize { get; set; }
    public int Placement { get; set; } 
    public decimal Percentage { get; set; } 
    public decimal CalculatedPayoutAmount => (Prize.TotalPool * Percentage) / 100;
    public Guid? WinningTeamId { get; set; }
    public Team? WinningTeam { get; set; }
}