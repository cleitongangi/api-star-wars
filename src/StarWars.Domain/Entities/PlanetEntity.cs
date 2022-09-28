namespace StarWars.Domain.Entities
{
    public class PlanetEntity
    {
        public int PlanetId { get; set; } // PlanetId (Primary key)
        public string Name { get; set; } = null!; // Name (length: 50)
        public string Climate { get; set; } = null!; // Climate (length: 50)
        public string Terrain { get; set; } = null!; // Terrain (length: 50)
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public bool Active { get; set; } // Active
        public DateTime? Deleted { get; set; } // Deleted

        // Reverse navigation        
        public virtual ICollection<FilmPlanetEntity> FilmPlanet { get; set; }

        public PlanetEntity()
        {
            FilmPlanet = new List<FilmPlanetEntity>();
        }

        public static class Factory
        {
            /// <summary>
            /// Create a new instance to insert entity in database
            /// </summary>
            /// <param name="name"></param>
            /// <param name="climate"></param>
            /// <param name="terrain"></param>
            /// <returns></returns>
            public static PlanetEntity CreateForAdd(int planetId, string name, string climate, string terrain, ICollection<FilmPlanetEntity> filmPlanet)
            {
                return new PlanetEntity()
                {
                    PlanetId = planetId,
                    Name = name,
                    Climate = climate,
                    Terrain = terrain,
                    Active = true,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    FilmPlanet = filmPlanet
                };
            }
        }
    }
}
