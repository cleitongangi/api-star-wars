using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Data;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Infra.Data.Context;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace StarWars.Infra.Data
{
    public static class MigrationManager
    {
        public static async Task ApplyMigrationAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<StarWarsDbContext>();
                        
            // Apply migrations in Database
            await appContext.Database.MigrateAsync();
        }
    }
}
