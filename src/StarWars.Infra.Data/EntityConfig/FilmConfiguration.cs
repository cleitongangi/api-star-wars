using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StarWars.Domain.Entities;

namespace StarWars.Infra.Data.EntityConfig
{
    public class FilmConfiguration : IEntityTypeConfiguration<FilmEntity>
    {
        public void Configure(EntityTypeBuilder<FilmEntity> builder)
        {
            builder.ToTable("Film");
            builder.HasKey(x => x.FilmId);

            builder.Property(x => x.FilmId).HasColumnName(@"FilmId").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.Director).HasColumnName(@"Director").HasColumnType("varchar(50)").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(x => x.ReleaseDate).HasColumnName(@"ReleaseDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Created).HasColumnName(@"Created").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Modified).HasColumnName(@"Modified").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.Active).HasColumnName(@"Active").HasColumnType("bit").IsRequired();
            builder.Property(x => x.Deleted).HasColumnName(@"Deleted").HasColumnType("datetime").IsRequired(false);
        }
    }
}
