using Cinema.Model;
using Cinema.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HallController : Controller
    {
        private readonly IHallService _hallService;

        public HallController(IHallService hallService)
        {
            _hallService = hallService ?? throw new ArgumentNullException(nameof(hallService));
        }

        [HttpPost]
        public async Task<IActionResult> AddHallAsync([FromBody] Hall hall)
        {
            try
            {
                await _hallService.AddHallAsync(hall);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHallsAsync()
        {
            try
            {
                var halls = await _hallService.GetAllHallsAsync();
                return Ok(halls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHallByIdAsync(Guid id)
        {
            try
            {
                var hall = await _hallService.GetHallByIdAsync(id);
                if (hall == null)
                {
                    return NotFound();
                }
                return Ok(hall);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        
        
        [HttpGet("halls")]
        public async Task<IActionResult> GetAvailableHallsAsync(DateOnly date, TimeOnly time, Guid movieId)
        {
            try
            {
                var halls = await _hallService.GetAvailableHallsAsync(date, time, movieId);
                
                return Ok(halls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("by-projection/{projectionId}")]
        public async Task<IActionResult> GetHallByProjectionIdAsync(Guid projectionId)
        {
            try
            {
                var hall = await _hallService.GetHallByProjectionIdAsync(projectionId);
                if (hall == null)
                {
                    return NotFound();
                }
                return Ok(hall);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHallAsync(Guid id, [FromBody] Hall hall)
        {
            if (id != hall.Id)
            {
                return BadRequest("Hall ID mismatch");
            }

            try
            {
                await _hallService.UpdateHallAsync(hall);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHallAsync(Guid id)
        {
            try
            {
                await _hallService.DeleteHallAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}