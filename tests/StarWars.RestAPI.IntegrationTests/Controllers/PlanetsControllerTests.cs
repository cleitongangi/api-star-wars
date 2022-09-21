using StarWars.Domain.Core.Pagination;
using StarWars.RestAPI.ApiResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.RestAPI.IntegrationTests.Controllers
{
    public class PlanetsControllerTests
    {
        [Fact]
        public async Task GetPlanets_DefaultPage_ReturnOk()
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
        public async Task GetPlanets_NonexistentPage_ReturnOk()
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
        public async Task GetPlanets_Page2_ReturnOk()
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
        public async Task GetPlanets_WithData_ReturnOk()
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
    }
}
