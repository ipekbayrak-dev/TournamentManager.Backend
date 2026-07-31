using FluentValidation;
using TournamentManager.Application.Dtos.Payment;

namespace TournamentManager.Application.Validators.Payment
{
    public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentRequestValidator()
        {
            RuleFor(x => x.TournamentEntryId)
                .NotEqual(Guid.Empty).WithMessage("Tournament entry Id cannot be empty.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency cannot be empty.")
                .Length(3).WithMessage("Currency must be exactly 3 characters.");

            RuleFor(x => x.StripeSessionId)
                .NotEmpty().WithMessage("Stripe session Id cannot be empty.");
        }
    }
}
