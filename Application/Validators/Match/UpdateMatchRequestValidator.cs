using FluentValidation;
using TournamentManager.Application.Dtos.Match;

namespace TournamentManager.Application.Validators.Match
{
    public class UpdateMatchRequestValidator : AbstractValidator<UpdateMatchRequest>
    {
        public UpdateMatchRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");

            RuleFor(x => x.TeamRadiantScore)
                .GreaterThanOrEqualTo(0).WithMessage("Radiant team score must be greater than or equal to 0.");

            RuleFor(x => x.TeamDireScore)
                .GreaterThanOrEqualTo(0).WithMessage("Dire team score must be greater than or equal to 0.");
        }
    }
}