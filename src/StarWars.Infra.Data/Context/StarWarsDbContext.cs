using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;
using StarWars.Infra.Data.EntityConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Infra.Data.Context
{
    public class StarWarsDbContext : DbContext
    {
        public DbSet<FilmEntity> Film { get; set; } = null!; // Film
        public DbSet<PlanetEntity> Planet { get; set; } = null!; // Planet


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FilmConfiguration());
            modelBuilder.ApplyConfiguration(new PlanetConfiguration());
        }
    }
}
