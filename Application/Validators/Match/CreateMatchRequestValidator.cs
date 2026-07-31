using FluentValidation;
using TournamentManager.Application.Dtos.Match;

namespace TournamentManager.Application.Validators.Match
{
    public class CreateMatchRequestValidator : AbstractValidator<CreateMatchRequest>
    {
        public CreateMatchRequestValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEqual(Guid.Empty).WithMessage("Tournament Id cannot be empty.");
            
            RuleFor(x => x.RoundNumber)
                .GreaterThan(0).WithMessage("Match round number must be greater than 0.");

            RuleFor(x => x.ScheduledAt)
                .Must(d => d > DateTime.UtcNow).WithMessage("Match schedule must be scheduled for the future.");
        }
    }
}