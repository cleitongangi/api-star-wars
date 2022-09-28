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
        Task<bool> HasFilmAsync(int filmId);
        Task<bool?> GetPlanetStatusAsync(int planetId);        
        Task<PagedResult<PlanetEntity>> ListPlanetsAsync(string? search = null, int page = 1);
        void ReactivePlanet(PlanetEntity entity);
    }
}
