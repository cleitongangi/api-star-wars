using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Infra.IoC.RestAPI
{
    public static class IoCWrapper
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StarWarsDb");
            Data.RegisterIoC.Register(services, connectionString);            
            Domain.RegisterIoC.Register(services);
        }
    }
}
