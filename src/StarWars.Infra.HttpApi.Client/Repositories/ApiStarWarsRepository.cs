using Microsoft.Extensions.Configuration;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Repositories;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace StarWars.Infra.HttpApi.Client.Repositories
{
    public class ApiStarWarsRepository : IApiStarWarsRepository
    {
        private readonly IConfiguration _configuration;

        public ApiStarWarsRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_configuration["AppSettings:SwapiUrl"])
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        public async Task<FilmEntity?> GetFilmToAddAsync(int filmId)
        {
            using var client = this.GetClient();

            HttpResponseMessage responseFilms = await client.GetAsync($"/api/films/{filmId}");
            if (responseFilms.IsSuccessStatusCode)
            {
                var film = await responseFilms.Content.ReadFromJsonAsync<JsonDocument>();
                if (film != null)
                {
                    return FilmEntity.Factory.CreateForAdd(
                            filmId,
                            name: film.RootElement.GetProperty("title").GetString() ?? throw new NullReferenceException("title can't be null"),
                            director: film.RootElement.GetProperty("director").GetString() ?? throw new NullReferenceException("director can't be null"),
                            releaseDate: film.RootElement.GetProperty("release_date").GetDateTime()
                        );
                }
            }
            return null;
        }

        public async Task<PlanetEntity?> GetPlanetToAddAsync(int planetId)
        {
            using var client = this.GetClient();

            HttpResponseMessage response = await client.GetAsync($"/api/planets/{planetId}");
            if (response.IsSuccessStatusCode)
            {
                var planet = await response.Content.ReadFromJsonAsync<JsonDocument>();
                if (planet != null)
                {
                    // Get related films
                    var relatedFilms = new List<FilmPlanetEntity>();
                    foreach (var filmUrl in planet.RootElement.GetProperty("films").EnumerateArray())
                    {
                        var url = filmUrl.GetString();
                        var startPosition = url![..^1].LastIndexOf('/') + 1;
                        if (int.TryParse(url[startPosition..^1], out var filmId))
                        {
                            relatedFilms.Add(new FilmPlanetEntity(filmId, planetId));
                        }
                        else
                        {
                            throw new FormatException($"Error when try parse filmId from films url ({url}) to int. FilmId: {url[startPosition..^1]}");
                        }
                    }
                    return PlanetEntity.Factory.CreateForAdd(
                            planetId,
                            name: planet.RootElement.GetProperty("name").GetString() ?? throw new NullReferenceException("name can't be null"),
                            climate: planet.RootElement.GetProperty("climate").GetString() ?? throw new NullReferenceException("climate can't be null"),
                            terrain: planet.RootElement.GetProperty("terrain").GetString() ?? throw new NullReferenceException("terrain can't be null"),
                            filmPlanet: relatedFilms
                        );
                }
            }
            return null;
        }
    }
}
