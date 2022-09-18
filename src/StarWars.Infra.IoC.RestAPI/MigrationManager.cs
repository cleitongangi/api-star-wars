using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Infra.Data.Context;

namespace StarWars.Infra.IoC.RestAPI
{
    public static class MigrationManager
    {
        public static void ApplyMigration(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<StarWarsDbContext>();
            appContext.Database.Migrate();
        }
    }
}
