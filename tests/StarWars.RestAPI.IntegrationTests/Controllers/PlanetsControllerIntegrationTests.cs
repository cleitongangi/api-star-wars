using StarWars.Domain.Core.Pagination;
using StarWars.RestAPI.ApiResponses;
using System.Net;
using System.Net.Http.Json;

namespace StarWars.RestAPI.IntegrationTests.Controllers
{
    public class PlanetsControllerIntegrationTests
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
            var result = await client.GetFromJsonAsync<PagedResult<Planet>>("/api/Planets?search=an");

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
            var result = await client.DeleteAsync("/api/Planets/2");

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
            var client = application.CreateClient();

            var firstDelete = await client.DeleteAsync("/api/Planets/1");
            Assert.NotNull(firstDelete);
            Assert.Equal(HttpStatusCode.NoContent, firstDelete.StatusCode);

            // Act
            var secondDelete = await client.DeleteAsync("/api/Planets/1");

            // Assert
            Assert.NotNull(secondDelete);
            Assert.Equal(HttpStatusCode.NotFound, secondDelete.StatusCode);
        }

        [Fact]
        public async Task ImportPlanet_Inexistent_ReturnNotFound()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.PostAsync("/api/Planets/9999", null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task ImportPlanet_ExistentPlanet_ReturnCreated()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            // Act
            var result = await client.PostAsync("/api/Planets/3", null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task ImportPlanet_Twice_ReturnConflict()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            var firstPost = await client.PostAsync("/api/Planets/4", null);
            Assert.NotNull(firstPost);
            Assert.Equal(HttpStatusCode.Created, firstPost.StatusCode);

            // Act
            var secondPost = await client.PostAsync("/api/Planets/4", null);

            // Assert
            Assert.NotNull(secondPost);
            Assert.Equal(HttpStatusCode.Conflict, secondPost.StatusCode);
        }

        [Fact]
        public async Task ImportPlanet_DisabledPlanet_ReturnReactivated()
        {
            // Arrange
            await using var application = new RestApiApplicationFactory();
            var client = application.CreateClient();

            var createPlanet = await client.PostAsync("/api/Planets/4", null);
            Assert.NotNull(createPlanet);
            Assert.Equal(HttpStatusCode.Created, createPlanet.StatusCode);

            var deletePlanet = await client.DeleteAsync("/api/Planets/4");
            Assert.NotNull(deletePlanet);
            Assert.Equal(HttpStatusCode.NoContent, deletePlanet.StatusCode);

            // Act
            var secondPost = await client.PostAsync("/api/Planets/4", null);

            // Assert
            Assert.NotNull(secondPost);
            Assert.Equal(HttpStatusCode.Created, secondPost.StatusCode);
        }
    }
}
