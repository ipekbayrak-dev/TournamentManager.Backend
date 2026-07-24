using Microsoft.Extensions.DependencyInjection;
using TournamentManager.Application.Features;
using TournamentManager.Application.Interfaces.Services;

namespace TournamentManager.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPrizeService, PrizeService>();
            services.AddScoped<IPrizeAllocationService, PrizeAllocationService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITournamentEntryService, TournamentEntryService>();
            return services;
        }
    }
}