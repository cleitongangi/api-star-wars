using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging;
using StarWars.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using StarWars.Infra.Data.Repositories;

namespace StarWars.Infra.Data
{
    public static class RegisterIoC
    {
        public static readonly LoggerFactory MyLoggerFactory = new(new[] { new DebugLoggerProvider() });

        public static void Register(IServiceCollection services, string posterrDbConnectionString)
        {
            #region Register DBContext            
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<StarWarsDbContext, StarWarsDbContext>();

            services.AddDbContext<StarWarsDbContext>(options =>
            {
                options
                    .UseSqlServer(posterrDbConnectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .UseLoggerFactory(MyLoggerFactory);
            });
            #endregion Register DBContext


            #region Register all repositories
            var repositoriesAssembly = typeof(PlanetRepository).Assembly;
            var repositoriesRegistrations =
                from type in repositoriesAssembly.GetExportedTypes()
                where type.Namespace == "StarWars.Infra.Data.Repositories"
                where type.GetInterfaces().Any()
                select new { Interface = type.GetInterfaces().FirstOrDefault(), Implementation = type };

            foreach (var reg in repositoriesRegistrations)
            {
                services.AddScoped(reg.Interface, reg.Implementation);
            }
            #endregion Register all repositories
        }
    }
}
