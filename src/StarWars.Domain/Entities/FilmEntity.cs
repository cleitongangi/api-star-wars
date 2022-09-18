namespace StarWars.Domain.Entities
{
    public class FilmEntity
    {
        public int FilmId { get; set; } // Id (Primary key)
        public string Name { get; set; } = null!; // Name (length: 50)
        public string Director { get; set; } = null!; // Director (length: 50)
        public DateTime ReleaseDate { get; set; } // ReleaseDate
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public bool Active { get; set; } // Active
        public DateTime? Deleted { get; set; } // Deleted

        // Reverse navigation

        /// <summary>
        /// Child Planet (Many-to-Many) mapped by table [FilmPlanet]
        /// </summary>
        public virtual ICollection<PlanetEntity> Planet { get; set; } // Many to many mapping

        public FilmEntity()
        {
            Planet = new List<PlanetEntity>();
        }
    }
}
