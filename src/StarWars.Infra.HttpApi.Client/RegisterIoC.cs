using Microsoft.Extensions.DependencyInjection;
using StarWars.Infra.HttpApi.Client.Repositories;

namespace StarWars.Infra.HttpApi.Client
{
    public static class RegisterIoC
    {
        public static void Register(IServiceCollection services)
        {
            #region Register all repositories
            var repositoriesAssembly = typeof(ApiStarWarsRepository).Assembly;
            var repositoriesRegistrations =
                from type in repositoriesAssembly.GetExportedTypes()
                where type.Namespace == "StarWars.Infra.HttpApi.Client.Repositories"
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
