namespace StarWars.Domain.Entities
{
    public class FilmEntity
    {
        public int FilmId { get; private set; } // FilmId (Primary key)
        public string Name { get; private set; } = null!; // Name (length: 50)
        public string Director { get; private set; } = null!; // Director (length: 50)
        public DateTime ReleaseDate { get; private set; } // ReleaseDate
        public DateTime Created { get; private set; } // Created
        public DateTime Modified { get; private set; } // Modified
        public bool Active { get; private set; } // Active
        public DateTime? Deleted { get; private set; } // Deleted

        // Reverse navigation                
        public virtual ICollection<FilmPlanetEntity> FilmPlanet { get; set; }

        public FilmEntity()
        {            
            FilmPlanet = new List<FilmPlanetEntity>();
        }

        public static class Factory
        {
            public static FilmEntity CreateForAdd(string name, string director, DateTime releaseDate)
            {
                return new FilmEntity()
                {
                    Name = name,
                    Director = director,
                    ReleaseDate = releaseDate,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Active = true
                };
            }
        }
    }
}
