using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Entities;

namespace StarWars.Domain.Interfaces.Repositories
{
    public interface IStarWarsRepository
    {
        Task AddFilmAsync(FilmEntity entity);
        Task AddFilmPlanetAsync(FilmPlanetEntity entity);
        Task AddPlanetAsync(PlanetEntity entity);
        Task<int> DisablePlanetAsync(int planetId);
        Task<PlanetEntity?> GetPlanetAsync(int planetId);
        Task<PagedResult<PlanetEntity>> ListPlanetsAsync(int page = 1);
    }
}
