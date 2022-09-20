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
        public async Task<IActionResult> Get(int page = 1)
        {
            var result = await _planetRepository.GetPlanetsAsync(page);
            
            return Ok(_mapper.Map<PagedResult<Planet>>(result));
        }
    }
}