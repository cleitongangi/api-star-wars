using StarWars.Domain.Entities;
using StarWars.Domain.Interfaces.Data;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Domain.Interfaces.Services;
using System.Data;

namespace StarWars.Domain.Services
{
    public class StarWarsService : IStarWarsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IApiStarWarsRepository _apiStarWarsRepository;
        private readonly IStarWarsRepository _starWarsRepository;

        public StarWarsService(IUnitOfWork unitOfWork, IApiStarWarsRepository apiStarWarsRepository, IStarWarsRepository starWarsRepository)
        {
            this._uow = unitOfWork;
            this._apiStarWarsRepository = apiStarWarsRepository;
            this._starWarsRepository = starWarsRepository;
        }

        public async Task AddPlanetAndFilmsAsync(PlanetEntity planet)
        {
            try
            {
                await _uow.BeginTransactionAsync();

                // Add films if doesn't exist in DB
                if (planet.FilmPlanet != null)
                {
                    await ImportFilmsIfDoesntExistInDbAsync(planet.FilmPlanet.Select(x => x.FilmId));
                }

                // Adds Planet and related FilmPlanet
                await _starWarsRepository.AddPlanetAsync(planet);
                
                // Save and commit trasaction
                await _uow.SaveChangesAsync();                                
                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        private async Task ImportFilmsIfDoesntExistInDbAsync(IEnumerable<int> filmsId)
        {
            foreach (var filmId in filmsId)
            {
                await ImportFilmIfDoesntExistInDbAsync(filmId);
            }
        }

        private async Task ImportFilmIfDoesntExistInDbAsync(int filmId)
        {
            if (await _starWarsRepository.HasFilmAsync(filmId))
            {
                return;
            }

            var film = await _apiStarWarsRepository.GetFilmToAddAsync(filmId);
            if (film != null)
            {
                await _starWarsRepository.AddFilmAsync(film);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
