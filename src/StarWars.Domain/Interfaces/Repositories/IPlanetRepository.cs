using StarWars.Domain.Entities;

namespace StarWars.Domain.Interfaces.Repositories
{
    public interface IPlanetRepository
    {
        Task<IEnumerable<PlanetEntity>> GetPlanets();
    }
}
