using TournamentManager.Domain.Enums;

namespace TournamentManager.Application.Dtos.Player
{
    public class UpdatePlayerRequest
    {
        public required string Handle { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string CountryCode { get; set; }
        public required DotaPosition Position { get; set; }
        public bool IsCaptain { get; set; }
        public string? SteamId { get; set; }
        public Guid TeamId { get; set; }
    }
}