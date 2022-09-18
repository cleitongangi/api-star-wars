using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;
using StarWars.Infra.Data.EntityConfig;

namespace StarWars.Infra.Data.Context
{
    public class StarWarsDbContext : DbContext
    {
        public DbSet<FilmEntity> Film { get; set; } = null!; // Film
        public DbSet<PlanetEntity> Planet { get; set; } = null!; // Planet

        public StarWarsDbContext(DbContextOptions<StarWarsDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FilmConfiguration());
            modelBuilder.ApplyConfiguration(new PlanetConfiguration());
        }
    }
}
