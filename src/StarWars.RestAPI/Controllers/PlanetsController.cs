using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StarWars.Domain.Core.Pagination;
using StarWars.Domain.Interfaces.Repositories;
using StarWars.RestAPI.ApiResponses;

namespace StarWars.RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlanetsController : ControllerBase
    {
        private readonly ILogger<PlanetsController> _logger;
        private readonly IMapper _mapper;
        private readonly IStarWarsRepository _planetRepository;

        public PlanetsController(ILogger<PlanetsController> logger, IMapper mapper, IStarWarsRepository planetRepository)
        {
            _logger = logger;
            this._mapper = mapper;
            this._planetRepository = planetRepository;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(PagedResult<Planet>), 200)]
        public async Task<IActionResult> ListPlanets(string? search = null, int page = 1)
        {
            var result = await _planetRepository.ListPlanetsAsync(search, page);

            return Ok(_mapper.Map<PagedResult<Planet>>(result));
        }

        [HttpGet("{planetId}")]
        [ProducesResponseType(typeof(Planet), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlanet(int planetId)
        {
            var result = await _planetRepository.GetPlanetAsync(planetId);
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
            var affectedRows = await _planetRepository.DisablePlanetAsync(planetId);
            if (affectedRows == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}