using StarWars.Domain.Entities;

namespace StarWars.Domain.Interfaces.Services
{
    public interface IStarWarsService
    {   
        Task AddPlanetAndFilmsAsync(PlanetEntity planet);
    }
}
