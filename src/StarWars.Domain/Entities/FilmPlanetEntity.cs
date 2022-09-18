namespace StarWars.Domain.Entities
{
    public class FilmPlanetEntity
    {
        public int FilmId { get; private set; } // FilmId (Primary key)
        public int PlanetId { get; private set; } // PlanetId (Primary key)

        // Reverse navigation
        public virtual FilmEntity Film { get; set; } = null!;
        public virtual PlanetEntity Planet { get; set; } = null!;

        protected FilmPlanetEntity() { } // Constructor for Entity Framework use

        public FilmPlanetEntity(int filmId, int planetId)
        {
            FilmId = filmId;
            PlanetId = planetId;
        }
    }
}
