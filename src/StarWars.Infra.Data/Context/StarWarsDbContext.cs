using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;
using StarWars.Infra.Data.EntityConfig;

namespace StarWars.Infra.Data.Context
{
    public class StarWarsDbContext : DbContext
    {
        public DbSet<FilmEntity> Film => Set<FilmEntity>(); // Film
        public DbSet<FilmPlanetEntity> FilmPlanet => Set<FilmPlanetEntity>(); // FilmPlanet
        public DbSet<PlanetEntity> Planet => Set<PlanetEntity>(); // Planet        

        public StarWarsDbContext(DbContextOptions<StarWarsDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FilmConfiguration());
            modelBuilder.ApplyConfiguration(new FilmPlanetConfiguration());
            modelBuilder.ApplyConfiguration(new PlanetConfiguration());
        }
    }
}
