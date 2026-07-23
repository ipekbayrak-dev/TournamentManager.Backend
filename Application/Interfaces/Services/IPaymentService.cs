using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Payment;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        public Task<Result<PaymentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<Result<ICollection<PaymentResponse>>> GetAllByTournamentEntryIdAsync(Guid tournamentEntryId, CancellationToken cancellationToken = default);
        public Task<Result<PaymentResponse>> CreateAsync(CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken = default);
        public Task<Result> UpdateAsync(UpdatePaymentRequest updatePaymentRequest, CancellationToken cancellationToken = default);
        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}