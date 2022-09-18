using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWars.Infra.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Director = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.FilmId);
                });

            migrationBuilder.CreateTable(
                name: "Planet",
                columns: table => new
                {
                    PlanetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Climate = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Terrain = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planet", x => x.PlanetId);
                });

            migrationBuilder.CreateTable(
                name: "FilmPlanet",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmPlanet", x => new { x.FilmId, x.PlanetId });
                    table.ForeignKey(
                        name: "FK_FilmPlanet_Film_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmPlanet_Planet_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planet",
                        principalColumn: "PlanetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmPlanet_PlanetId",
                table: "FilmPlanet",
                column: "PlanetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmPlanet");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "Planet");
        }
    }
}
