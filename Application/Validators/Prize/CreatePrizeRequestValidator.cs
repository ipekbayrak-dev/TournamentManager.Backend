using FluentValidation;
using TournamentManager.Application.Dtos.Prize;

namespace TournamentManager.Application.Validators.Prize
{
    public class CreatePrizeRequestValidator : AbstractValidator<CreatePrizeRequest>
    {
        public CreatePrizeRequestValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEqual(Guid.Empty).WithMessage("Tournament Id cannot be empty.");

            RuleFor(x => x.TotalPool)
                .GreaterThan(0).WithMessage("Total pool must be greater than 0.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency cannot be empty.")
                .Length(3).WithMessage("Currency must be exactly 3 characters.");
        }
    }
}
