﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StarWars.Infra.Data.Context;

#nullable disable

namespace StarWars.Infra.Data.Migrations
{
    [DbContext(typeof(StarWarsDbContext))]
    [Migration("20220918154620_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FilmEntityPlanetEntity", b =>
                {
                    b.Property<int>("FilmId")
                        .HasColumnType("int");

                    b.Property<int>("PlanetId")
                        .HasColumnType("int");

                    b.HasKey("FilmId", "PlanetId");

                    b.HasIndex("PlanetId");

                    b.ToTable("FilmPlanet", (string)null);
                });

            modelBuilder.Entity("StarWars.Domain.Entities.FilmEntity", b =>
                {
                    b.Property<int>("FilmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FilmId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FilmId"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit")
                        .HasColumnName("Active");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime")
                        .HasColumnName("Created");

                    b.Property<DateTime?>("Deleted")
                        .HasColumnType("datetime")
                        .HasColumnName("Deleted");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Director");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime")
                        .HasColumnName("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Name");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime")
                        .HasColumnName("ReleaseDate");

                    b.HasKey("FilmId");

                    b.ToTable("Film", (string)null);
                });

            modelBuilder.Entity("StarWars.Domain.Entities.PlanetEntity", b =>
                {
                    b.Property<int>("PlanetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PlanetId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlanetId"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit")
                        .HasColumnName("Active");

                    b.Property<string>("Climate")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Climate");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime")
                        .HasColumnName("Created");

                    b.Property<DateTime?>("Deleted")
                        .HasColumnType("datetime")
                        .HasColumnName("Deleted");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime")
                        .HasColumnName("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Name");

                    b.Property<string>("Terrain")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Terrain");

                    b.HasKey("PlanetId");

                    b.ToTable("Planet", (string)null);
                });

            modelBuilder.Entity("FilmEntityPlanetEntity", b =>
                {
                    b.HasOne("StarWars.Domain.Entities.FilmEntity", null)
                        .WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StarWars.Domain.Entities.PlanetEntity", null)
                        .WithMany()
                        .HasForeignKey("PlanetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}