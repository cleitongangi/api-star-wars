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

        [HttpPost("{planetId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ImportPlanet(int planetId)
        {
            var planetFromApi = await _apiStarWarsRepository.GetPlanetToAddAsync(planetId);
            if (planetFromApi == null)
            {
                return NotFound();
            }

            var planetStatus = await _starWarsRepository.GetPlanetStatusAsync(planetId);
            if (planetStatus == null)
            { 
                // Insert planet
                await _starWarsService.AddPlanetAndFilmsAsync(planetFromApi);                
            }
            else if (planetStatus.Value)
            { 
                // Planet already was imported
                ModelState.AddModelError("planetId", "PlanetId informed already exists.");
                return Conflict(ModelState);
            }
            else
            { 
                // Reactive planet.
                _starWarsRepository.ReactivePlanet(planetFromApi);
                await _uow.SaveChangesAsync();                
            }

            return new CreatedAtRouteResult(nameof(GetPlanet), new { planetId }, null);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(PagedResult<Planet>), 200)]
        public async Task<IActionResult> ListPlanets(string? search = null, int page = 1)
        {
            var result = await _starWarsRepository.ListPlanetsAsync(search, page);

            return Ok(_mapper.Map<PagedResult<Planet>>(result));
        }

        [HttpGet("{planetId}", Name = nameof(GetPlanet))]
        [ProducesResponseType(typeof(Planet), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlanet(int planetId)
        {
            var result = await _starWarsRepository.GetPlanetAsync(planetId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Planet>(result));
        }

        [HttpDelete("{planetId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DisablePlanet(int planetId)
        {
            var affectedRows = await _starWarsRepository.DisablePlanetAsync(planetId);
            if (affectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}