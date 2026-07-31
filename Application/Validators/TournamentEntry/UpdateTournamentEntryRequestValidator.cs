using FluentValidation;
using TournamentManager.Application.Dtos.TournamentEntry;

namespace TournamentManager.Application.Validators.TournamentEntry
{
    public class UpdateTournamentEntryRequestValidator : AbstractValidator<UpdateTournamentEntryRequest>
    {
        public UpdateTournamentEntryRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");
        }
    }
}
