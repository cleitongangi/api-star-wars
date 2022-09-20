using StarWars.Domain.Interfaces.GlobalSettings;
using StarWars.RestAPI.GlobalSettings;

namespace StarWars.RestAPI
{
    public static class DependencyInjectorStartup
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigSettings, ConfigSettings>();

            Infra.IoC.RestAPI.IoCWrapper.Register(services, configuration);
        }
    }
}
