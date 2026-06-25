using TournamentManager.Domain.Common;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid TournamentEntryId { get; set; }
    public required TournamentEntry TournamentEntry { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public required string StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public DateTime? PaidAt { get; set; }
}
