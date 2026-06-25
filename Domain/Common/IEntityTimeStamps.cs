namespace TournamentManager.Domain.Common
{
    public interface IEntityTimeStamps
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        DateTimeOffset? DeletedAt { get; set; }
    }
}
