namespace StarWars.Domain.Entities
{
    public class PlanetEntity
    {
        public int PlanetId { get; set; } // Id (Primary key)
        public string Name { get; set; } = null!; // Name (length: 50)
        public string Climate { get; set; } = null!; // Climate (length: 50)
        public string Terrain { get; set; } = null!; // Terrain (length: 50)
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public bool Active { get; set; } // Active
        public DateTime? Deleted { get; set; } // Deleted

        // Reverse navigation

        /// <summary>
        /// Child Film (Many-to-Many) mapped by table [FilmPlanet]
        /// </summary>
        public virtual ICollection<FilmEntity> Film { get; set; } // Many to many mapping

        public PlanetEntity()
        {
            Film = new List<FilmEntity>();
        }
    }
}
