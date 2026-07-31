using FluentValidation;
using TournamentManager.Application.Dtos.TournamentEntry;

namespace TournamentManager.Application.Validators.TournamentEntry
{
    public class CreateTournamentEntryRequestValidator : AbstractValidator<CreateTournamentEntryRequest>
    {
        public CreateTournamentEntryRequestValidator()
        {
            RuleFor(x => x.TournamentId)
                .NotEqual(Guid.Empty).WithMessage("Tournament Id cannot be empty.");

            RuleFor(x => x.TeamId)
                .NotEqual(Guid.Empty).WithMessage("Team Id cannot be empty.");
        }
    }
}
