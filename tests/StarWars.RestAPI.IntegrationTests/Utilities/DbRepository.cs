using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;
using StarWars.Infra.Data.Context;

namespace StarWars.RestAPI.IntegrationTests.Utilities
{
    internal class DbRepository
    {
        internal static void InitializeDbForTests(StarWarsDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            dbContext.Film.Add(FilmEntity.Factory.CreateForAdd(1, "Film 1", "Tests 1", DateTime.Now));
            dbContext.Film.Add(FilmEntity.Factory.CreateForAdd(2, "Film 2", "Tests 2", DateTime.Now));
            dbContext.SaveChanges();

            dbContext.Planet.Add(PlanetEntity.Factory.CreateForAdd(1, "Planet 1", "Climate 1", "Terrain 1", new List<FilmPlanetEntity>()));
            dbContext.Planet.Add(PlanetEntity.Factory.CreateForAdd(2, "Planet 2", "Climate 2", "Terrain 2", new List<FilmPlanetEntity>()));
            dbContext.SaveChanges();

            dbContext.FilmPlanet.Add(new FilmPlanetEntity(1, 1));
            dbContext.FilmPlanet.Add(new FilmPlanetEntity(1, 2));
            dbContext.SaveChanges();
        }
    }
}
