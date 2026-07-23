using System.Security.Claims;

namespace TournamentManager.Application.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateToken(IEnumerable<Claim> claims);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}