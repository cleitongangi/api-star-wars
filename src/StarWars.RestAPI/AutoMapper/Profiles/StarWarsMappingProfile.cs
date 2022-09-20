using AutoMapper;
using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Entities;
using StarWars.RestAPI.ApiResponses;
using StarWars.RestAPI.AutoMapper.CustomConverters;

namespace StarWars.RestAPI.AutoMapper.Profiles
{
    public class StarWarsMappingProfile : Profile
    {
        public StarWarsMappingProfile()
        {
            CreateMap<PlanetEntity, Planet>(MemberList.None)
                .ForMember(dest => dest.Films,
                    opt => opt.MapFrom(src => src.FilmPlanet
                        .Select(x => new Film()
                        {
                            FilmId = x.Film.FilmId,
                            Name = x.Film.Name,
                            Director = x.Film.Director,
                            ReleaseDate = x.Film.ReleaseDate
                        })
                    )
                );

            CreateMap<PagedResult<PlanetEntity>, PagedResult<Planet>>(MemberList.None)
                .ConvertUsing<PagedResultConverter<PlanetEntity, Planet>>();
        }
    }
}
