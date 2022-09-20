namespace StarWars.Domain.Entities
{
    public class FilmPlanetEntity
    {
        public int FilmId { get; set; } // FilmId (Primary key)
        public int PlanetId { get; set; } // PlanetId (Primary key)

        // Reverse navigation
        public virtual FilmEntity Film { get; set; } = null!;
        public virtual PlanetEntity Planet { get; set; } = null!;

        public FilmPlanetEntity(int filmId, int planetId)
        {
            FilmId = filmId;
            PlanetId = planetId;
        }
        public FilmPlanetEntity()
        {
        }
    }
}
