using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;

namespace StarWars.Infra.Data.EntityConfig
{
    public class FilmPlanetConfiguration : IEntityTypeConfiguration<FilmPlanetEntity>
    {
        public void Configure(EntityTypeBuilder<FilmPlanetEntity> builder)
        {
            builder.ToTable("FilmPlanet");
            builder.HasKey(x => new { x.FilmId, x.PlanetId });

            builder.Property(x => x.FilmId).HasColumnName(@"FilmId").HasColumnType("int").IsRequired();
            builder.Property(x => x.PlanetId).HasColumnName(@"PlanetId").HasColumnType("int").IsRequired();

            builder.HasOne(p => p.Film).WithMany(p => p.FilmPlanet).HasForeignKey(x => x.FilmId);
            builder.HasOne(p => p.Planet).WithMany(p => p.FilmPlanet).HasForeignKey(x => x.PlanetId);
        }
    }
}
