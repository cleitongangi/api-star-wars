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
        public async Task ListPlanets_WithData_ReturnOk()
        {
            // Arrange            
            _repositoryMock.Setup(repo => repo.ListPlanetsAsync(It.IsAny<int>()))
                .ReturnsAsync(new PagedResult<PlanetEntity>(new List<PlanetEntity>(), 1, 10));

            _mapperMock.Setup(map => map.Map<PagedResult<Planet>>(It.IsAny<PagedResult<PlanetEntity>>()))
                .Returns(new PagedResult<Planet>(new List<Planet>(), 1, 10));

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.ListPlanets(1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPlanet_WithoutData_ReturnNotFound()
        {
            // Arrange            
            _repositoryMock.Setup(repo => repo.GetPlanetAsync(It.IsAny<int>()))
                .ReturnsAsync((PlanetEntity?)null);

            _mapperMock.Setup(map => map.Map<Planet>(It.IsAny<PlanetEntity>()))
                .Returns(new Planet());

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.GetPlanet(1);
            var notFoundResult = result as NotFoundResult;

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetPlanet_WithData_ReturnOk()
        {
            // Arrange            
            _repositoryMock.Setup(repo => repo.GetPlanetAsync(It.IsAny<int>()))
                .ReturnsAsync(new PlanetEntity());

            _mapperMock.Setup(map => map.Map<Planet>(It.IsAny<PlanetEntity>()))
                .Returns(new Planet());

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.GetPlanet(1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DisablePlanet_NonexistentPlanet_ReturnNotFound()
        {
            // Arrange            
            _repositoryMock.Setup(repo => repo.DisablePlanetAsync(It.IsAny<int>()))
                .ReturnsAsync(0);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.DisablePlanet(1);
            var notFound = result as NotFoundResult;

            // Assert
            Assert.NotNull(notFound);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public async Task DisablePlanet_ExistingPlanet_ReturnNoContent()
        {
            // Arrange            
            _repositoryMock.Setup(repo => repo.DisablePlanetAsync(It.IsAny<int>()))
                .ReturnsAsync(1);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _repositoryMock.Object);

            // Act
            var result = await controller.DisablePlanet(1);
            var noContent = result as NoContentResult;

            // Assert
            Assert.NotNull(noContent);
            Assert.Equal(204, noContent.StatusCode);
        }
    }
}