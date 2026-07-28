using FluentValidation.Results;

namespace TournamentManager.Application.Common
{
    internal static class ValidationExtensions
    {
        internal static string ToErrorMessage(this ValidationResult result) =>
            string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
    }
}
