using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Data;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Domain.Interfaces.Services;
using StarWars.RestAPI.ApiResponses;
using StarWars.RestAPI.Controllers;

namespace StarWars.RestAPI.Tests.Controllers
{
    public class PlanetsControllerTests
    {

        private readonly Mock<IStarWarsRepository> _starWarsRepositoryMock = new();
        private readonly Mock<IApiStarWarsRepository> _apiStarWarsRepositoryMock = new();
        private readonly Mock<ILogger<PlanetsController>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IStarWarsService> _starWarsServiceMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

        [Fact]
        public async Task ListPlanets_WithData_ReturnOk()
        {
            // Arrange            
            _starWarsRepositoryMock.Setup(repo => repo.ListPlanetsAsync(null, It.IsAny<int>()))
                .ReturnsAsync(new PagedResult<PlanetEntity>(new List<PlanetEntity>(), 1, 10));

            _mapperMock.Setup(map => map.Map<PagedResult<Planet>>(It.IsAny<PagedResult<PlanetEntity>>()))
                .Returns(new PagedResult<Planet>(new List<Planet>(), 1, 10));

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.ListPlanets(null, 1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ListPlanets_WithSearch_ReturnOk()
        {
            // Arrange            
            _starWarsRepositoryMock.Setup(repo => repo.ListPlanetsAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new PagedResult<PlanetEntity>(new List<PlanetEntity>(), 1, 10));

            _mapperMock.Setup(map => map.Map<PagedResult<Planet>>(It.IsAny<PagedResult<PlanetEntity>>()))
                .Returns(new PagedResult<Planet>(new List<Planet>(), 1, 10));

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.ListPlanets("test", 1);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetPlanet_WithoutData_ReturnNotFound()
        {
            // Arrange            
            _starWarsRepositoryMock.Setup(repo => repo.GetPlanetAsync(It.IsAny<int>()))
                .ReturnsAsync((PlanetEntity?)null);

            _mapperMock.Setup(map => map.Map<Planet>(It.IsAny<PlanetEntity>()))
                .Returns(new Planet());

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

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
            _starWarsRepositoryMock.Setup(repo => repo.GetPlanetAsync(It.IsAny<int>()))
                .ReturnsAsync(new PlanetEntity());

            _mapperMock.Setup(map => map.Map<Planet>(It.IsAny<PlanetEntity>()))
                .Returns(new Planet());

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

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
            _starWarsRepositoryMock.Setup(repo => repo.DisablePlanetAsync(It.IsAny<int>()))
                .ReturnsAsync(0);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

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
            _starWarsRepositoryMock.Setup(repo => repo.DisablePlanetAsync(It.IsAny<int>()))
                .ReturnsAsync(1);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.DisablePlanet(1);
            var noContent = result as NoContentResult;

            // Assert
            Assert.NotNull(noContent);
            Assert.Equal(204, noContent.StatusCode);
        }

        [Fact]
        public async Task ImportPlanet_Inexistent_ReturnNotFound()
        {
            // Arrange
            _apiStarWarsRepositoryMock.Setup(repo => repo.GetPlanetToAddAsync(It.IsAny<int>()))
                .ReturnsAsync((PlanetEntity?)null);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.ImportPlanet(999);
            var notFound = result as NotFoundResult;

            // Assert
            Assert.NotNull(notFound);
            Assert.Equal(404, notFound.StatusCode);            
        }

        [Fact]
        public async Task ImportPlanet_Existent_ReturnOk()
        {
            // Arrange            
            _apiStarWarsRepositoryMock.Setup(repo => repo.GetPlanetToAddAsync(It.IsAny<int>()))
                .ReturnsAsync(new PlanetEntity());

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.ImportPlanet(999);
            var createdResult = result as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
            _starWarsServiceMock.Verify(mock => mock.AddPlanetAndFilmsAsync(It.IsAny<PlanetEntity>()), Times.Once());
        }

        [Fact]
        public async Task ImportPlanet_Twice_ReturnConflict()
        {
            // Arrange            
            _apiStarWarsRepositoryMock.Setup(repo => repo.GetPlanetToAddAsync(It.IsAny<int>()))
                .ReturnsAsync(new PlanetEntity());

            _starWarsRepositoryMock.Setup(repo => repo.GetPlanetStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.ImportPlanet(999);
            var conflictResult = result as ConflictObjectResult;

            // Assert
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task ImportPlanet_DeletedPlanet_ReturnOk()
        {
            // Arrange            
            _apiStarWarsRepositoryMock.Setup(repo => repo.GetPlanetToAddAsync(It.IsAny<int>()))
                .ReturnsAsync(new PlanetEntity());

            _starWarsRepositoryMock.Setup(repo => repo.GetPlanetStatusAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var controller = new PlanetsController(_loggerMock.Object, _mapperMock.Object, _starWarsRepositoryMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsServiceMock.Object, _uowMock.Object);

            // Act
            var result = await controller.ImportPlanet(999);
            var createdResult = result as CreatedAtRouteResult;

            // Assert
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
            _starWarsRepositoryMock.Verify(mock => mock.ReactivePlanet(It.IsAny<PlanetEntity>()), Times.Once());
        }
    }
}