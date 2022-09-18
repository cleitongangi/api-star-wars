using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Data;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Infra.Data.Context;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace StarWars.Infra.Data
{
    public static class MigrationManager
    {
        public static async Task ApplyMigrationAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<StarWarsDbContext>();

            // Checks if it's the first migration.
            var isFirstMigration = (await appContext
                .Database
                .GetPendingMigrationsAsync())
                .Any(x => x.EndsWith("InitialCreate"));

            // Apply migrations in Database
            await appContext.Database.MigrateAsync();

            if (isFirstMigration)
            {
                var apiUrl = configuration["AppSettings:SwapiUrl"];
                if (!string.IsNullOrEmpty(apiUrl)) // If is not defined, it does not import data
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IStarWarsRepository>();

                    // Populate database from https://swapi.dev/ API
                    await PopulateDbFromStarWarsApiAsync(apiUrl, scope);
                }
            }
        }

        public static async Task PopulateDbFromStarWarsApiAsync(string apiUrl, IServiceScope scope)
        {
            var starWarsRepository = scope.ServiceProvider.GetRequiredService<IStarWarsRepository>();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            using var client = new HttpClient();

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            await ImportPlanetsAsync(starWarsRepository, uow, client);
            await ImportFilmsAsync(starWarsRepository, uow, client);
        }

        private static async Task ImportFilmsAsync(IStarWarsRepository starWarsRepository, IUnitOfWork uow, HttpClient client)
        {
            string? filmsApiUrl = "/api/films";
            do
            {
                HttpResponseMessage responseFilms = await client.GetAsync(filmsApiUrl);
                if (responseFilms.IsSuccessStatusCode)
                {
                    var films = await responseFilms.Content.ReadFromJsonAsync<JsonDocument>();
                    if (films != null)
                    {
                        foreach (var film in films!.RootElement.GetProperty("results").EnumerateArray())
                        {
                            var title = film.GetProperty("title").GetString() ?? throw new NullReferenceException("title can't be null");
                            var director = film.GetProperty("director").GetString() ?? throw new NullReferenceException("director can't be null");
                            var releaseDate = film.GetProperty("release_date").GetDateTime();

                            var newFilmEntity = FilmEntity.Factory.CreateForAdd(title, director, releaseDate);
                            await starWarsRepository.AddFilmAsync(newFilmEntity);
                            await uow.SaveChangesAsync(); // This save is necessary to generate FilmId used bellow

                            foreach (var planetUrl in film.GetProperty("planets").EnumerateArray())
                            {
                                var url = planetUrl.GetString();
                                var startPosition = url![..^1].LastIndexOf('/') + 1;
                                if (int.TryParse(url[startPosition..^1], out var planetId))
                                {
                                    var newFilmPlanetEntity = new FilmPlanetEntity(newFilmEntity.FilmId, planetId);
                                    await starWarsRepository.AddFilmPlanetAsync(newFilmPlanetEntity);
                                }
                                else
                                {
                                    throw new FormatException($"Error when try parse planetId from planets url ({url}) to int. PlanetId: {url[startPosition..^1]}");
                                }
                            }
                            await uow.SaveChangesAsync();
                        }

                        filmsApiUrl = films!.RootElement.GetProperty("next").GetString();
                    }
                }
            } while (filmsApiUrl != null);
        }

        private static async Task ImportPlanetsAsync(IStarWarsRepository starWarsRepository, IUnitOfWork uow, HttpClient client)
        {
            // Import planets            
            string? planetsApiUrl = "/api/planets";
            do
            {
                HttpResponseMessage response = await client.GetAsync(planetsApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var planets = await response.Content.ReadFromJsonAsync<JsonDocument>();
                    if (planets != null)
                    {
                        foreach (var planet in planets!.RootElement.GetProperty("results").EnumerateArray())
                        {
                            var name = planet.GetProperty("name").GetString() ?? throw new NullReferenceException("name can't be null");
                            var climate = planet.GetProperty("climate").GetString() ?? throw new NullReferenceException("climate can't be null");
                            var terrain = planet.GetProperty("terrain").GetString() ?? throw new NullReferenceException("terrain can't be null");

                            var newPlanetEntity = PlanetEntity.Factory.CreateForAdd(name, climate, terrain);
                            await starWarsRepository.AddPlanetAsync(newPlanetEntity);
                        }
                        await uow.SaveChangesAsync();

                        planetsApiUrl = planets!.RootElement.GetProperty("next").GetString();
                    }
                }
            } while (planetsApiUrl != null);
        }
    }
}
