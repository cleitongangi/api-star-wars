using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;

namespace StarWars.Infra.Data.EntityConfig
{
    public class PlanetConfiguration : IEntityTypeConfiguration<PlanetEntity>
    {
        public void Configure(EntityTypeBuilder<PlanetEntity> builder)
        {
            builder.ToTable("Planet");
            builder.HasKey(x => x.PlanetId);

            builder.Property(x => x.PlanetId).HasColumnName(@"PlanetId").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Climate).HasColumnName(@"Climate").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Terrain).HasColumnName(@"Terrain").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Created).HasColumnName(@"Created").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Modified).HasColumnName(@"Modified").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Active).HasColumnName(@"Active").HasColumnType("bit").IsRequired();
            builder.Property(x => x.Deleted).HasColumnName(@"Deleted").HasColumnType("datetime").IsRequired(false);
        }
    }
}
