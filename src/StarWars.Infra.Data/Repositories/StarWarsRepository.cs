using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.GlobalSettings;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Infra.Data.Context;

namespace StarWars.Infra.Data.Repositories
{
    public class StarWarsRepository : IStarWarsRepository
    {
        private readonly StarWarsDbContext _db;
        private readonly IConfigSettings _configSettings;

        public StarWarsRepository(StarWarsDbContext starWarsDbContext, IConfigSettings configSettings)
        {
            this._db = starWarsDbContext;
            this._configSettings = configSettings;
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

        public async Task<PagedResult<PlanetEntity>> GetPlanetsAsync(int page = 1)
        {
            var query = _db.Planet
                .Select(x => new PlanetEntity()
                {
                    PlanetId = x.PlanetId,
                    Name = x.Name,
                    Climate = x.Climate,
                    Terrain = x.Terrain,
                    FilmPlanet = x.FilmPlanet.Select(fp => new FilmPlanetEntity()
                    {
                        Film = new FilmEntity()
                        {
                            FilmId = fp.Film.FilmId,
                            Name = fp.Film.Name,
                            Director = fp.Film.Director,
                            ReleaseDate = fp.Film.ReleaseDate
                        }
                    }).ToList()
                });
                        
            var pageSize = _configSettings.DefaultPaginationSize;
            var skip = (page - 1) * pageSize;
            var data = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResult<PlanetEntity>(data, page, pageSize);
        }
    }
}
