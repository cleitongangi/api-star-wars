using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Infra.Data.Context;

namespace StarWars.Infra.Data.Repositories
{
    public class StarWarsRepository : IStarWarsRepository
    {
        private readonly StarWarsDbContext _db;

        public StarWarsRepository(StarWarsDbContext starWarsDbContext)
        {
            this._db = starWarsDbContext;
        }

        public async Task AddPlanetAsync(PlanetEntity entity)
        {
            await _db.Planet.AddAsync(entity);            
        }

        public async Task AddFilmAsync(FilmEntity entity)
        {
            await _db.Film.AddAsync(entity);
        }

        public async Task AddFilmPlanetAsync(FilmPlanetEntity entity)
        {
            await _db.FilmPlanet.AddAsync(entity);
        }

        public async Task<IEnumerable<PlanetEntity>> GetPlanetsAsync()
        {
            return await _db.Planet.ToListAsync();
        }
    }
}
