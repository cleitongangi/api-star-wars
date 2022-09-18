using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Infra.Data.Context;

namespace StarWars.Infra.Data.Repositories
{
    public class PlanetRepository : IPlanetRepository
    {
        private readonly StarWarsDbContext _db;

        public PlanetRepository(StarWarsDbContext starWarsDbContext)
        {
            this._db = starWarsDbContext;
        }

        public async Task<IEnumerable<PlanetEntity>> GetPlanets()
        {
            return await _db.Planet.ToListAsync();
        }
    }
}
