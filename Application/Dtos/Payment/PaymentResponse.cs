using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Payment
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public Guid TournamentEntryId { get; set; }
        public decimal Amount { get; set; }
        public required string Currency { get; set; } 
        public PaymentStatus Status { get; set; } 
        public required string StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}