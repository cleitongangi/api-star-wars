using StarWars.Domain.Entities;

namespace StarWars.Domain.Interfaces.Repositories
{
    public interface IApiStarWarsRepository
    {
        Task<FilmEntity?> GetFilmToAddAsync(int filmId);
        Task<PlanetEntity?> GetPlanetToAddAsync(int planetId);
    }
}
