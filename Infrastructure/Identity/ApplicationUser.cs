using Microsoft.AspNetCore.Identity;
using TournamentManager.Domain.Entities;

namespace TournamentManager.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? PlayerId { get; set; }
        public Player? Player { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}