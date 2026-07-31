using FluentValidation;
using TournamentManager.Application.Dtos.Payment;

namespace TournamentManager.Application.Validators.Payment
{
    public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
    {
        public UpdatePaymentRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");
        }
    }
}
