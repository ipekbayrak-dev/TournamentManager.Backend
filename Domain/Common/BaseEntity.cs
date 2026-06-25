namespace TournamentManager.Domain.Common
{
    public class BaseEntity : IEntityTimeStamps
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsActive => !DeletedAt.HasValue;
    }
}