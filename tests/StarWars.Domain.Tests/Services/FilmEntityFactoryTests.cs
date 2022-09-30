using Moq;
using StarWars.Domain.Entities;

namespace StarWars.Domain.Tests.Services
{
    public class FilmEntityFactoryTests
    {
        [Fact]
        public void CreateForAdd_ValidData_Ok()
        {
            // Arrange
            var filmId = int.MaxValue;
            var name = "test";
            var director = "director test";
            var releaseDate = DateTime.Now;

            // Act
            var entity = FilmEntity.Factory.CreateForAdd(filmId, name, director, releaseDate);

            // Assert            
            Assert.NotNull(entity);
            Assert.Equal(filmId, entity.FilmId);
            Assert.Equal(name, entity.Name);
            Assert.Equal(director, entity.Director);
            Assert.Equal(releaseDate, entity.ReleaseDate);
        }
    }
}