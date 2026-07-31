using FluentValidation;
using TournamentManager.Application.Dtos.PrizeAllocation;

namespace TournamentManager.Application.Validators.PrizeAllocation
{
    public class UpdatePrizeAllocationRequestValidator : AbstractValidator<UpdatePrizeAllocationRequest>
    {
        public UpdatePrizeAllocationRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty).WithMessage("Id cannot be empty.");

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
