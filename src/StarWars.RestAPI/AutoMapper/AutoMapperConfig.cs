using AutoMapper;
using StarWars.RestAPI.AutoMapper.Profiles;

namespace StarWars.RestAPI.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StarWarsMappingProfile());
            })
            .AssertConfigurationIsValid();
        }
    }
}
