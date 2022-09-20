using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using StarWars.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.RestAPI.IntegrationTests
{
    internal class RestApiApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<StarWarsDbContext>));
                services.AddDbContext<StarWarsDbContext>(options =>
                        options
                        .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=StarWars;Trusted_Connection=True;MultipleActiveResultSets=true")
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    );
            });

            return base.CreateHost(builder);
        }
    }
}
