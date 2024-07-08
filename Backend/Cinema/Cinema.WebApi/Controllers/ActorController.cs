using Cinema.Model;
using Cinema.Service.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;

        public ActorController(IActorService actorService)
        {
            _actorService = actorService ?? throw new ArgumentNullException(nameof(actorService));
        }

        [HttpPost]
        public async Task<IActionResult> AddActorAsync([FromBody] Actor actor)
        {
            await _actorService.AddActorAsync(actor);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActorsAsync()
        {
            var actors = await _actorService.GetAllActorsAsync();
            return Ok(actors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActorByIdAsync(Guid id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(actor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActorAsync(Guid id, [FromBody] Actor actor)
        {
            if (id != actor.Id)
            {
                return BadRequest("Actor ID mismatch");
            }

            try
            {
                await _actorService.UpdateActorAsync(actor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActorAsync(Guid id)
        {
            try
            {
                await _actorService.DeleteActorAsync(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
