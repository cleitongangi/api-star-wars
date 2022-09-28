using Microsoft.Data.SqlClient;
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

        public async Task<bool?> GetPlanetStatusAsync(int planetId)
        {
            var planet = await _db.Planet.FirstOrDefaultAsync(x => x.PlanetId == planetId);
            if (planet != null)
            {
                return planet.Active;
            }
            return null;
        }

        public async Task AddPlanetAsync(PlanetEntity entity)
        {
            await _db.Planet.AddAsync(entity);
        }

        public void ReactivePlanet(PlanetEntity entity)
        {
            _db.Attach(entity);
            _db.Entry(entity).Property(r => r.Modified).IsModified = true;
            _db.Entry(entity).Property(r => r.Deleted).IsModified = true;
            _db.Entry(entity).Property(r => r.Active).IsModified = true;
        }

        public async Task<bool> HasFilmAsync(int filmId)
        {
            return await _db.Film.AnyAsync(x => x.FilmId == filmId);
        }

        public async Task AddFilmAsync(FilmEntity entity)
        {
            await _db.Film.AddAsync(entity);
        }

        public async Task AddFilmPlanetAsync(FilmPlanetEntity entity)
        {
            await _db.FilmPlanet.AddAsync(entity);
        }

        public async Task<PagedResult<PlanetEntity>> ListPlanetsAsync(string? search = null, int page = 1)
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

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            var pageSize = _configSettings.DefaultPaginationSize;
            var skip = (page - 1) * pageSize;
            var data = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResult<PlanetEntity>(data, page, pageSize);
        }

        public async Task<PlanetEntity?> GetPlanetAsync(int planetId)
        {
            return await _db.Planet
                .Where(x => x.PlanetId == planetId)
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
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> DisablePlanetAsync(int planetId)
        {
            // [Active] = 1 to ensure that won't disable twice even with concurrency
            var query = @"update [Planet]
                set [Active] = 0
                    ,[Modified] = GETDATE()
                    ,[Deleted] = GETDATE()
                where [PlanetId] = @planetId
                    and [Active] = 1";

            return await _db.Database.ExecuteSqlRawAsync(query, new SqlParameter("@planetId", planetId));
        }
    }
}
