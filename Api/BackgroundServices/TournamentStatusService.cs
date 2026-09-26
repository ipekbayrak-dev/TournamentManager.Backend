using TournamentManager.Application.Interfaces.Repositories;
using TournamentManager.Domain.Enums;

namespace TournamentManager.Api.BackgroundServices
{
    public class TournamentStatusService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TournamentStatusService> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

        public TournamentStatusService(IServiceScopeFactory scopeFactory, ILogger<TournamentStatusService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateStatusesAsync(stoppingToken);
                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task UpdateStatusesAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ITournamentRepository>();
                var now = DateTime.UtcNow;

                var tournaments = await repo.GetAllAsync(
                    t => t.Status != TournamentStatus.Completed && t.Status != TournamentStatus.Cancelled,
                    enabledTracking: true,
                    cancellationToken: cancellationToken
                );

                var toUpdate = new List<Domain.Entities.Tournament>();

                foreach (var t in tournaments)
                {
                    var newStatus = t.Status;

                    if (t.EndDate < now)
                        newStatus = TournamentStatus.Completed;
                    else if (t.StartDate <= now && (t.Status == TournamentStatus.Upcoming || t.Status == TournamentStatus.RegistrationOpen))
                        newStatus = TournamentStatus.Ongoing;

                    if (newStatus != t.Status)
                    {
                        _logger.LogInformation("Tournament '{Name}': {Old} → {New}", t.Name, t.Status, newStatus);
                        t.Status = newStatus;
                        toUpdate.Add(t);
                    }
                }

                if (toUpdate.Count > 0)
                    await repo.UpdateRangeAsync(toUpdate);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error updating tournament statuses");
            }
        }
    }
}
