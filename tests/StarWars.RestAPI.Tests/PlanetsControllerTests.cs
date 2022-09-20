using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.RestAPI.ApiResponses;
using StarWars.RestAPI.Controllers;

namespace StarWars.RestAPI.Tests
{
    public class PlanetsControllerTests
    {

        private readonly Mock<IStarWarsRepository> _repositoryMock = new();
        private readonly Mock<ILogger<PlanetsController>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        [Fact]
        public async Task GetPlanets_WithData_ReturnOk()
        {
            // Arrange            
            _repositoryMock.Setup(repo => repo.GetPlanetsAsync(It.IsAny<int>()))
                .ReturnsAsync(new PagedResult<PlanetEntity>(new List<PlanetEntity>(), 1, 10));

            _mapperMock.Setup(map => map.Map<PagedResult<Planet>>(It.IsAny<PagedResult<PlanetEntity>>()))
                .Returns(new PagedResult<Planet>(new List<Planet>(), 1, 10));
                
            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.Get(1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}