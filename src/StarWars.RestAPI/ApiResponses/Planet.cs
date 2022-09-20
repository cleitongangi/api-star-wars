namespace StarWars.RestAPI.ApiResponses
{
    public class Planet
    {
        public int PlanetId { get; set; }
        public string Name { get; set; } = null!;
        public string Climate { get; set; } = null!;
        public string Terrain { get; set; } = null!;                         
        public virtual ICollection<Film> Films { get; set; }

        public Planet()
        {
            Films = new List<Film>();
        }
    }
}
