using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Interfaces.Data;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.Domain.Interfaces.Services;
using StarWars.RestAPI.ApiResponses;
using System.Numerics;

namespace StarWars.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlanetsController : ControllerBase
    {
        private readonly ILogger<PlanetsController> _logger;
        private readonly IMapper _mapper;
        private readonly IStarWarsRepository _starWarsRepository;
        private readonly IApiStarWarsRepository _apiStarWarsRepository;
        private readonly IStarWarsService _starWarsService;
        private readonly IUnitOfWork _uow;

        public PlanetsController(
            ILogger<PlanetsController> logger,
            IMapper mapper,
            IStarWarsRepository starWarsRepository,
            IApiStarWarsRepository apiStarWarsRepository,
            IStarWarsService starWarsService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this._mapper = mapper;
            this._starWarsRepository = starWarsRepository;
            this._apiStarWarsRepository = apiStarWarsRepository;
            this._starWarsService = starWarsService;
            this._uow = unitOfWork;
        }

        /// <summary>
        /// Import a planet from public Star Wars API https://swapi.dev/
        /// </summary>
        /// <param name="planetId"></param>
        /// <returns>A newly imported planet</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Planets/1
        ///
        ///  Is not necessary post any json content, only planetId in URL
        /// </remarks>
        /// <response code="201">Returns the newly imported planet</response>
        /// <response code="404">If not found the planet in https://swapi.dev/ API to import</response>
        /// <response code="409">If planet was already imported</response>
        [HttpPost("{planetId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ImportPlanet(int planetId)
        {
            var planetFromApi = await _apiStarWarsRepository.GetPlanetToAddAsync(planetId);
            if (planetFromApi == null)
            {
                _logger.LogInformation("PlanetId '{id}' not found to import.", planetId);
                return NotFound();
            }

            var planetStatus = await _starWarsRepository.GetPlanetStatusAsync(planetId);
            if (planetStatus == null)
            {
                // Insert planet
                await _starWarsService.AddPlanetAndFilmsAsync(planetFromApi);
                _logger.LogInformation("PlanetId '{id}' imported.", planetId);
            }
            else if (planetStatus.Value)
            {
                // Planet already was imported
                ModelState.AddModelError("planetId", "PlanetId informed already exists.");
                _logger.LogInformation("PlanetId '{id}' informed already exists.", planetId);
                return Conflict(ModelState);
            }
            else
            {
                // Reactive planet.
                _starWarsRepository.ReactivePlanet(planetFromApi);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("PlanetId '{id}' reactivated.", planetId);
            }

            return new CreatedAtRouteResult(nameof(GetPlanet), new { planetId }, null);
        }

        /// <summary>
        /// List planets and search planets by name
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <returns>A paginated list of planets</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Planets?search=a&amp;page=2
        ///
        /// </remarks>
        /// <response code="200">Returns a list of planets with pagination</response>
        [HttpGet()]
        [ProducesResponseType(typeof(PagedResult<Planet>), 200)]
        public async Task<IActionResult> ListPlanets(string? search = null, int page = 1)
        {
            var result = await _starWarsRepository.ListPlanetsAsync(search, page);

            return Ok(_mapper.Map<PagedResult<Planet>>(result));
        }

        /// <summary>
        /// Get a planet by id
        /// </summary>
        /// <param name="planetId"></param>
        /// <returns>A planet with related films</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Planets/1
        ///
        /// </remarks>
        /// <response code="200">Returns a planet with related films</response>
        /// <response code="404">If the planet doesn't exist</response>
        [HttpGet("{planetId}", Name = nameof(GetPlanet))]
        [ProducesResponseType(typeof(Planet), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlanet(int planetId)
        {
            var result = await _starWarsRepository.GetPlanetAsync(planetId);
            if (result == null)
            {
                _logger.LogInformation("PlanetId '{id}' not found.", planetId);
                return NotFound();
            }

            return Ok(_mapper.Map<Planet>(result));
        }

        /// <summary>
        /// Delete a planet by id
        /// </summary>
        /// <param name="planetId"></param>
        /// <returns>No content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/Planets/1
        ///
        /// </remarks>
        /// <response code="404">If the planet doesn't exist</response>
        /// <response code="204">Returns no content</response>
        [HttpDelete("{planetId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DisablePlanet(int planetId)
        {
            var affectedRows = await _starWarsRepository.DisablePlanetAsync(planetId);
            if (affectedRows == 0)
            {
                _logger.LogInformation("PlanetId '{id}' not found to disable.", planetId);
                return NotFound();
            }

            _logger.LogInformation("PlanetId '{id}' disabled.", planetId);
            return NoContent();
        }
    }
}