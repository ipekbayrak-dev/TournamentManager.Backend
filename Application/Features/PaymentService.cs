using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Payment;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application.Features
{
    public class PaymentService : IPaymentService
    {
        public Task<Result<PaymentResponse>> CreateAsync(CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ICollection<PaymentResponse>>> GetAllByTournamentEntryIdAsync(Guid tournamentEntryId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PaymentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateAsync(UpdatePaymentRequest updatePaymentRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}