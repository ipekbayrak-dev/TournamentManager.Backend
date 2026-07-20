using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Payment
{
    public class CreatePaymentRequest
    {
        public Guid TournamentEntryId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public required string StripeSessionId { get; set; }
    }
}