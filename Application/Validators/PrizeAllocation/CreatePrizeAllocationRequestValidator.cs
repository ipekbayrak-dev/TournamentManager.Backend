using FluentValidation;
using TournamentManager.Application.Dtos.PrizeAllocation;

namespace TournamentManager.Application.Validators.PrizeAllocation
{
    public class CreatePrizeAllocationRequestValidator : AbstractValidator<CreatePrizeAllocationRequest>
    {
        public CreatePrizeAllocationRequestValidator()
        {
            RuleFor(x => x.PrizeId)
                .NotEqual(Guid.Empty).WithMessage("Prize Id cannot be empty.");

            RuleFor(x => x.Placement)
                .GreaterThan(0).WithMessage("Placement must be greater than 0.");

            RuleFor(x => x.Percentage)
                .GreaterThan(0).WithMessage("Percentage must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Percentage cannot exceed 100.");
        }
    }
}
