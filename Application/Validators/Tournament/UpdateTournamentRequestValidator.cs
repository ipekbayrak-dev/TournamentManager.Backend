using FluentValidation;
using TournamentManager.Application.Dtos.Tournament;

namespace TournamentManager.Application.Validators.Tournament
{
    public class UpdateTournamentRequestValidator : AbstractValidator<UpdateTournamentRequest>
    {
        public UpdateTournamentRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tournament name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .Must(d => d > DateTime.UtcNow).WithMessage("Start date must be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
        }
    }
}
