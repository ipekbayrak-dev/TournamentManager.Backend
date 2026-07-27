using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Payment;
using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Features
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        private static PaymentResponse MapToResponse(Payment payment)
        {
            return new PaymentResponse
            {
                Id = payment.Id,
                TournamentEntryId = payment.TournamentEntryId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Status = payment.Status,
                StripeSessionId = payment.StripeSessionId,
                StripePaymentIntentId = payment.StripePaymentIntentId,
                PaidAt = payment.PaidAt
            };
        }
        public async Task<Result<PaymentResponse>> CreateAsync(CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken = default)
        {
            if (createPaymentRequest.TournamentEntryId == Guid.Empty)
            {
                return Result<PaymentResponse>.Failure("Invalid tournament entry ID.");
            }

            var payment = new Payment
            {
                TournamentEntryId = createPaymentRequest.TournamentEntryId,
                Amount = createPaymentRequest.Amount,
                Currency = createPaymentRequest.Currency,
                Status = PaymentStatus.Pending,
                StripeSessionId = createPaymentRequest.StripeSessionId,
            };

            await _paymentRepository.AddAsync(payment);
            return Result<PaymentResponse>.Success(MapToResponse(payment));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var payment = await _paymentRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (payment is null)
            {
                return Result.Failure("Payment not found");
            }

            await _paymentRepository.DeleteAsync(payment);

            return Result.Success();
        }

        public async Task<Result<ICollection<PaymentResponse>>> GetAllByTournamentEntryIdAsync(Guid tournamentEntryId, CancellationToken cancellationToken = default)
        {
            var payments = await _paymentRepository.GetAllAsync(
                x => x.TournamentEntryId == tournamentEntryId,
                cancellationToken: cancellationToken
            );

            var response = payments.Select(MapToResponse).ToList();

            return Result<ICollection<PaymentResponse>>.Success(response);
        }

        public async Task<Result<PaymentResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var payment = await _paymentRepository.GetAsync(x => x.Id == id, cancellationToken: cancellationToken);
            
            if (payment is null)
            {
                return Result<PaymentResponse>.Success(null);
            }

            return Result<PaymentResponse>.Success(MapToResponse(payment));
        }

        public async Task<Result> UpdateAsync(UpdatePaymentRequest updatePaymentRequest, CancellationToken cancellationToken = default)
        {
            var payment = await _paymentRepository.GetAsync(x => x.Id == updatePaymentRequest.Id, cancellationToken: cancellationToken);

            if (payment is null)
            {
                return Result.Failure("Invalid Id");
            }

            payment.Status = updatePaymentRequest.Status;
            payment.StripePaymentIntentId = updatePaymentRequest.StripePaymentIntentId;
            payment.PaidAt = updatePaymentRequest.PaidAt;

            await _paymentRepository.UpdateAsync(payment);

            return Result.Success();
        }
    }
}