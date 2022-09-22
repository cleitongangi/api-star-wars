using StarWars.Domain.Core.Pagination;
using StarWars.RestAPI.ApiResponses;
using StarWars.RestAPI.IntegrationTests.Utilities;
using System.Net;
using System.Net.Http.Json;

namespace StarWars.RestAPI.IntegrationTests.Controllers
{
    public class PlanetsControllerTests
    {
        [Fact]
        public async Task ListPlanets_DefaultPage_ReturnOk()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<PagedResult<Planet>>("/api/Planets");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(1, result?.CurrentPage);
        }

        [Fact]
        public async Task ListPlanets_NonexistentPage_ReturnOk()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<PagedResult<Planet>>("/api/Planets?page=999");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Results.Count);
            Assert.Equal(999, result?.CurrentPage);
        }

        [Fact]
        public async Task ListPlanets_Page2_ReturnOk()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<PagedResult<Planet>>("/api/Planets?page=2");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(2, result?.CurrentPage);
        }

        [Fact]
        public async Task ListPlanets_WithSearch_ReturnOk()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<PagedResult<Planet>>("/api/Planets?search=ar");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(1, result?.CurrentPage);
        }

        [Fact]
        public async Task GetPlanet_ActivePlanet_ReturnOk()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.GetFromJsonAsync<Planet>("/api/Planets/1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result?.PlanetId);
        }

        [Fact]
        public async Task GetPlanet_NonexistentPlanet_ReturnNotFound()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.GetAsync("/api/Planets/9999");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result?.StatusCode);
        }

        [Fact]
        public async Task DisablePlanet_ActivePlanet_ReturnNoContent()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();            
            var client = application.CreateClient();

            // Act
            var result = await client.DeleteAsync("/api/Planets/10");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DisablePlanet_NonexistentPlanet_ReturnNotFound()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.DeleteAsync("/api/Planets/99999");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DisablePlanet_Twice_ReturnNotFound()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            await new DbRepository(application).RecreateDbAsync();
            var client = application.CreateClient();

            var firstDelete = await client.DeleteAsync("/api/Planets/5");
            Assert.NotNull(firstDelete);
            Assert.Equal(HttpStatusCode.NoContent, firstDelete.StatusCode);

            // Act
            var secondDelete = await client.DeleteAsync("/api/Planets/5");

            // Assert
            Assert.NotNull(secondDelete);
            Assert.Equal(HttpStatusCode.NotFound, secondDelete.StatusCode);
        }
    }
}
