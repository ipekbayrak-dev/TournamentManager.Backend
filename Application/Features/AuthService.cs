using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TournamentManager.Application.Common;
using TournamentManager.Application.Dtos.Auth;
using TournamentManager.Application.Interfaces.Services;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Application.Features
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }
        public Task<Result<TokenResponse>> RefreshAsync(TokenResponse tokenResponse, CancellationToken cancellationToken = default)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenResponse.AccessToken);

            if (principal is null)
                return Task.FromResult(Result<TokenResponse>.Failure("Invalid token"));

            var newAccessToken = _tokenService.GenerateToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            return Task.FromResult(Result<TokenResponse>.Success(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }));
        }

        public Task<Result> RevokeAsync(string userId, CancellationToken cancellationToken = default)
        {
            // refresh token storage not implemented — no-op for now
            return Task.FromResult(Result.Success());
        }

        public async Task<Result<TokenResponse>> SignInAsync(SignInRequest signInRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(signInRequest.Email);
            if (user is null)
            {
                return Result<TokenResponse>.Failure("Invalid credentials");
            }

            var isCorrect = await _userManager.CheckPasswordAsync(user, signInRequest.Password);
            if (!isCorrect)
            {
                return Result<TokenResponse>.Failure("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new("displayName", user.DisplayName ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var accessToken = _tokenService.GenerateToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Result<TokenResponse>.Success(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        public async Task<Result> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default)
        {
            if (await _userManager.FindByEmailAsync(signUpRequest.Email) is not null)
            {
                return Result.Failure("Email already in use");
            }

            var user = new ApplicationUser
            {
                UserName = signUpRequest.Email,
                Email = signUpRequest.Email,
                FirstName = signUpRequest.FirstName,
                LastName = signUpRequest.LastName,
                DisplayName = signUpRequest.DisplayName
            };

            var createResult = await _userManager.CreateAsync(user, signUpRequest.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result.Failure(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Player);

            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Result.Failure(roleErrors);
            }

            return Result.Success();
        }
    }
}