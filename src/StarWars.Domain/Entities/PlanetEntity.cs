namespace StarWars.Domain.Entities
{
    public class PlanetEntity
    {
        public int PlanetId { get; private set; } // PlanetId (Primary key)
        public string Name { get; private set; } = null!; // Name (length: 50)
        public string Climate { get; private set; } = null!; // Climate (length: 50)
        public string Terrain { get; private set; } = null!; // Terrain (length: 50)
        public DateTime Created { get; private set; } // Created
        public DateTime Modified { get; private set; } // Modified
        public bool Active { get; private set; } // Active
        public DateTime? Deleted { get; private set; } // Deleted

        // Reverse navigation        
        public virtual ICollection<FilmPlanetEntity> FilmPlanet { get; set; }

        public PlanetEntity()
        {            
            FilmPlanet = new List<FilmPlanetEntity>();
        }

        public static class Factory
        {
            public static PlanetEntity CreateForAdd(string name, string climate, string terrain)
            {
                return new PlanetEntity()
                {
                    Name = name,
                    Climate = climate,
                    Terrain = terrain,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Active = true
                };
            }
        }
    }
}
