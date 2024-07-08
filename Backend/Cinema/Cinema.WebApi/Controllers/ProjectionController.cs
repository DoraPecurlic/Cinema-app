using AutoMapper;
using Cinema.Model;
using Cinema.Service.Common;
using DTO.ProjectionModel;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectionController : ControllerBase
    {
        private readonly IProjectionService _projectionService;
        private readonly IMapper _mapper;

        public ProjectionController(IProjectionService projectionService, IMapper mapper)
        {
            _projectionService = projectionService ?? throw new ArgumentNullException(nameof(projectionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProjectionRest>>> GetAllProjectionsAsync()
        {
            var projections = await _projectionService.GetAllProjectionsAsync();
            var projectionRests = _mapper.Map<IEnumerable<GetProjectionRest>>(projections);
            return Ok(projectionRests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProjectionRest>> GetProjectionByIdAsync(Guid id)
        {
            var projection = await _projectionService.GetProjectionByIdAsync(id);
            if (projection == null)
            {
                return NotFound();
            }

            var projectionRest = _mapper.Map<GetProjectionRest>(projection);
            return Ok(projectionRest);
        }
        
        [HttpGet("movie/{id}")]
        public async Task<ActionResult<IEnumerable<GetProjectionRest>>> GetProjectionsByMovieIdAsync(Guid id)
        {
            var projections = await _projectionService.GetProjectionsByMovieIdAsync(id);
            if (projections == null || !projections.Any())
            {
                return NotFound();
            }

            var projectionRest = _mapper.Map<IEnumerable<GetProjectionRest>>(projections);
            return Ok(projectionRest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectionAsync(Guid id, [FromBody] PutProjectionRest putProjectionRest)
        {
            if (putProjectionRest == null)
            {
                return BadRequest("Projection data is null");
            }

            var existingProjection = await _projectionService.GetProjectionByIdAsync(id);
            if (existingProjection == null)
            {
                return NotFound();
            }

            _mapper.Map(putProjectionRest, existingProjection);

            await _projectionService.UpdateProjectionAsync(existingProjection);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectionAsync([FromBody] PostProjectionRest postProjectionRest)
        {
            if (postProjectionRest == null)
            {
                return BadRequest("Projection data is null");
            }

            var projection = _mapper.Map<Projection>(postProjectionRest);
            await _projectionService.AddProjectionAsync(projection);

            var createdProjection = await _projectionService.GetProjectionByIdAsync(projection.Id);
            var projectionRest = _mapper.Map<GetProjectionRest>(createdProjection);

            return Created(string.Empty, projectionRest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectionAsync(Guid id)
        {
            var projection = await _projectionService.GetProjectionByIdAsync(id);
            if (projection == null)
            {
                return NotFound();
            }
            await _projectionService.DeleteProjectionAsync(id);
            return NoContent();
        }
    }
}
