using FluentValidation;
using TournamentManager.Application.Dtos.Player;

namespace TournamentManager.Application.Validators
{
    public class CreatePlayerRequestValidator : AbstractValidator<CreatePlayerRequest>
    {
        public CreatePlayerRequestValidator()
        {
            RuleFor(x => x.Handle)
                .NotEmpty().WithMessage("Handle is required.")
                .MaximumLength(50).WithMessage("Handle cannot exceed 50 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Length(2).WithMessage("Country code must be exactly 2 characters.");

            RuleFor(x => x.TeamId)
                .NotEqual(Guid.Empty).WithMessage("A valid team ID is required.");
        }
    }
}
