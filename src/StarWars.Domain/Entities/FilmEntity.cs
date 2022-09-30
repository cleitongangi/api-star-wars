namespace StarWars.Domain.Entities
{
    public class FilmEntity
    {
        public int FilmId { get; set; } // FilmId (Primary key)
        public string Name { get; set; } = null!; // Name (length: 50)
        public string Director { get; set; } = null!; // Director (length: 50)
        public DateTime ReleaseDate { get; set; } // ReleaseDate
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public bool Active { get; set; } // Active
        public DateTime? Deleted { get; set; } // Deleted

        // Reverse navigation                
        public virtual ICollection<FilmPlanetEntity> FilmPlanet { get; set; }

        public FilmEntity()
        {            
            FilmPlanet = new List<FilmPlanetEntity>();
        }

        public static class Factory
        {
            /// <summary>
            /// Create a new instance to insert entity in database
            /// </summary>
            /// <param name="name"></param>
            /// <param name="director"></param>
            /// <param name="releaseDate"></param>
            /// <returns></returns>
            public static FilmEntity CreateForAdd(int filmId, string name, string director, DateTime releaseDate)
            {
                return new FilmEntity()
                {
                    FilmId = filmId,
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
