using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentManager.Infrastructure.Identity;
using TournamentManager.Infrastructure.Persistence;
using TournamentManager.Infrastructure.Persistence.Contracts;
using TournamentManager.Infrastructure.Persistence.Repositories;

namespace TournamentManager.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            }); 

            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IPrizeAllocationRepository, PrizeAllocationRepository>();
            services.AddScoped<IPrizeRepository, PrizeRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITournamentEntryRepository, TournamentEntryRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();

            return services;
        }
    }
}