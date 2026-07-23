using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Auth;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default);
        public Task<Result<TokenResponse>> SignInAsync(SignInRequest signInRequest, CancellationToken cancellationToken = default);
        public Task<Result<TokenResponse>> RefreshAsync(TokenResponse tokenResponse, CancellationToken cancellationToken = default);
        public Task<Result> RevokeAsync(string userId, CancellationToken cancellationToken = default);
    }
}