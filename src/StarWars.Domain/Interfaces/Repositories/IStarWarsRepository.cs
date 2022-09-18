using StarWars.Domain.Entities;

namespace StarWars.Domain.Interfaces.Repositories
{
    public interface IStarWarsRepository
    {
        Task AddFilmAsync(FilmEntity entity);
        Task AddFilmPlanetAsync(FilmPlanetEntity entity);
        Task AddPlanetAsync(PlanetEntity entity);
        Task<IEnumerable<PlanetEntity>> GetPlanetsAsync();
    }
}
