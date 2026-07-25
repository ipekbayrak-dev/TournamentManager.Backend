using TournamentManager.Domain.Common; 
using TournamentManager.Domain.Enums;

namespace TournamentManager.Domain.Entities; 

public class TournamentEntry : BaseEntity 
{
    public Guid TournamentId { get; set; }
    public Tournament? Tournament { get; set; }
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public DateTime RegisteredAt { get; set; }
    public int? Seed { get; set; }
    public EntryStatus Status { get; set; } = EntryStatus.Pending;
    public Payment? Payment { get; set; }
}