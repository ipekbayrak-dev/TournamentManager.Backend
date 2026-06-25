using TournamentManager.Domain.Common; 
using TournamentManager.Domain.Enums;

namespace TournamentManager.Domain.Entities; 

public class TournamentEntry : BaseEntity 
{
    public Guid TournamentId { get; set; }
    public required Tournament Tournament { get; set; }
    public Guid TeamId { get; set; }
    public required Team Team { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public int? Seed { get; set; }
    public EntryStatus Status { get; set; } = EntryStatus.Pending;
    public Payment? Payment { get; set; }
}