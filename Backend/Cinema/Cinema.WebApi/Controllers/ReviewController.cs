using AutoMapper;
using Cinema.Model;
using Cinema.Service.Common;
using DTO.ReviewModel;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetReviewRest>>> GetAllReviewsAsync()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            var reviewRests = _mapper.Map<IEnumerable<GetReviewRest>>(reviews);
            return Ok(reviewRests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetReviewRest>> GetReviewByIdAsync(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            var reviewRest = _mapper.Map<GetReviewRest>(review);
            return Ok(reviewRest);
        }
        
        [HttpGet("movie/{id}")]
        public async Task<ActionResult<GetReviewRest>> GetReviewsByMovieIdAsync(Guid id)
        {
            var reviews = await _reviewService.GetReviewsByMovieIdAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }
            var reviewRests = _mapper.Map<IEnumerable<GetReviewRest>>(reviews);
           
            return Ok(reviewRests);
        }

        [HttpPost]
        public async Task<ActionResult<GetReviewRest>> AddReviewAsync([FromBody] PostReviewRest postReviewRest)
        {
            if (postReviewRest == null)
            {
                return BadRequest("Review data is null");
            }

            var review = _mapper.Map<Review>(postReviewRest);
            await _reviewService.AddReviewAsync(review);

            var createdReview = await _reviewService.GetReviewByIdAsync(review.Id);
            var reviewRest = _mapper.Map<GetReviewRest>(createdReview);

            return Created(string.Empty, reviewRest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReviewAsync(Guid id, [FromBody] PutReviewRest putReviewRest)
        {
            if (putReviewRest == null)
            {
                return BadRequest("Review data is null");
            }

            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            _mapper.Map(putReviewRest, existingReview);
            existingReview.DateUpdated = DateTime.UtcNow;

            await _reviewService.UpdateReviewAsync(existingReview);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewAsync(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
    }
}
