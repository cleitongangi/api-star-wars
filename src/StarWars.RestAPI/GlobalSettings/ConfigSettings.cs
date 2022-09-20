using StarWars.Domain.Interfaces.GlobalSettings;

namespace StarWars.RestAPI.GlobalSettings
{
    public class ConfigSettings : IConfigSettings
    {
        public int DefaultPaginationSize { get; private set; }

        public ConfigSettings(IConfiguration configuration)
        {
            DefaultPaginationSize = configuration.GetSection("AppSettings").GetValue<int>("DefaultPaginationSize");
        }
    }
}
