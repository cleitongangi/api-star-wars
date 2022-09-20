namespace StarWars.RestAPI.ApiResponses
{
    public class Film
    {
        public int FilmId { get; set; }
        public string Name { get; set; } = null!;
        public string Director { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
    }
}
