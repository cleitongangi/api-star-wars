using Moq;
using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Data;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Domain.Services;

namespace StarWars.Domain.Tests.Services
{
    public class StarWarsServiceTest
    {

        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IStarWarsRepository> _starWarsRepositoryMock = new();
        private readonly Mock<IApiStarWarsRepository> _apiStarWarsRepositoryMock = new();

        [Fact]
        public async Task AddPlanetAndFilmsAsync_FilmPlanetEmpty_NoErrors()
        {
            // Arrange
            var service = new StarWarsService(_uowMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsRepositoryMock.Object);
            var planet = PlanetEntity.Factory.CreateForAdd(1, "Teste Service", "Test", "terrain", new List<FilmPlanetEntity>());

            // Act
            var exception = await Record.ExceptionAsync(() => service.AddPlanetAndFilmsAsync(planet));

            // Assert
            _starWarsRepositoryMock.Verify(x => x.AddPlanetAsync(planet), Times.Once());
            _uowMock.Verify(x => x.CommitAsync(), Times.Once());
            _apiStarWarsRepositoryMock.Verify(x => x.GetFilmToAddAsync(It.IsAny<int>()), Times.Never());
            Assert.Null(exception);
        }

        [Fact]
        public async Task AddPlanetAndFilmsAsync_FilmPlanetNotEmpty_NoErrors()
        {
            // Arrange
            _apiStarWarsRepositoryMock.Setup(rep => rep.GetFilmToAddAsync(It.IsAny<int>()))
                .ReturnsAsync(new FilmEntity());

            _starWarsRepositoryMock.Setup(rep => rep.HasFilmAsync(1)).ReturnsAsync(true);
            _starWarsRepositoryMock.Setup(rep => rep.HasFilmAsync(3)).ReturnsAsync(false);
            _starWarsRepositoryMock.Setup(rep => rep.HasFilmAsync(4)).ReturnsAsync(false);

            var service = new StarWarsService(_uowMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsRepositoryMock.Object);

            var films = new List<FilmPlanetEntity>(){
                new FilmPlanetEntity(1, 1),
                new FilmPlanetEntity(3, 1),
                new FilmPlanetEntity(4, 1)
            };
            var planet = PlanetEntity.Factory.CreateForAdd(1, "Teste Service", "Test", "terrain", films);

            // Act
            var exception = await Record.ExceptionAsync(() => service.AddPlanetAndFilmsAsync(planet));

            // Assert
            _starWarsRepositoryMock.Verify(x => x.AddPlanetAsync(planet), Times.Once());
            _uowMock.Verify(x => x.CommitAsync(), Times.Once());
            _apiStarWarsRepositoryMock.Verify(x => x.GetFilmToAddAsync(1), Times.Never());
            _apiStarWarsRepositoryMock.Verify(x => x.GetFilmToAddAsync(3), Times.Once());
            _apiStarWarsRepositoryMock.Verify(x => x.GetFilmToAddAsync(4), Times.Once());
            _starWarsRepositoryMock.Verify(x => x.AddFilmAsync(It.IsAny<FilmEntity>()), Times.Exactly(2));
        }

        [Fact]
        public async Task AddPlanetAndFilmsAsync_ErrorInRepository_ThrowException()
        {
            // Arrange
            _starWarsRepositoryMock.Setup(rep => rep.AddPlanetAsync(It.IsAny<PlanetEntity>())).ThrowsAsync(new Exception());

            var service = new StarWarsService(_uowMock.Object, _apiStarWarsRepositoryMock.Object, _starWarsRepositoryMock.Object);
            var planet = PlanetEntity.Factory.CreateForAdd(1, "Teste Service", "Test", "terrain", new List<FilmPlanetEntity>());

            // Act
            var exception = await Record.ExceptionAsync(() => service.AddPlanetAndFilmsAsync(planet));

            // Assert
            Assert.NotNull(exception);
            _starWarsRepositoryMock.Verify(x => x.AddPlanetAsync(planet), Times.Once());            
            _uowMock.Verify(x => x.CommitAsync(), Times.Never());
            _uowMock.Verify(x => x.RollbackAsync(), Times.Once());
        }
    }
}