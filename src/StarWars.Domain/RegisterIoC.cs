using Microsoft.Extensions.DependencyInjection;
using StarWars.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Domain
{
    public static class RegisterIoC
    {
        public static void Register(IServiceCollection services)
        {
            #region Register all Services
            var servicesAssembly = typeof(IStarWarsService).Assembly;
            var serviceRegistrations =
                from type in servicesAssembly.GetExportedTypes()
                where type.Namespace == "StarWars.Domain.Services"
                where type.GetInterfaces().Any()
                select new { Interface = type.GetInterfaces().Single(), Implementation = type };

            foreach (var reg in serviceRegistrations)
                services.AddScoped(reg.Interface, reg.Implementation);
            #endregion
        }
    }
}
