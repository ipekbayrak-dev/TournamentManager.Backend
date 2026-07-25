using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Payment
{
    public class UpdatePaymentRequest
    {
        public Guid Id { get; set; }
        public PaymentStatus Status { get; set; } 
        public string? StripePaymentIntentId { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}