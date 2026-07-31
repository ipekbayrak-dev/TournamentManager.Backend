using FluentValidation;
using TournamentManager.Application.Dtos.Team;

namespace TournamentManager.Application.Validators.Team
{
    public class CreateTeamRequestValidator : AbstractValidator<CreateTeamRequest>
    {
        public CreateTeamRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Team name cannot be empty.")
                .MaximumLength(100).WithMessage("Team name length cannot exceed 100.");
            
            RuleFor(x => x.Handle)
                .NotEmpty().WithMessage("Team handle cannot be empty.")
                .MaximumLength(50).WithMessage("Team handle length cannot exceed 50.");

            RuleFor(x => x.Logo)
                .NotEmpty().WithMessage("Team logo cannot be empty.");
        }
    }
}