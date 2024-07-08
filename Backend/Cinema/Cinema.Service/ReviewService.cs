using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;

namespace Cinema.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            return await _reviewRepository.GetReviewByIdAsync(id);
        }
        
        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(Guid id)
        {
            return await _reviewRepository.GetReviewsByMovieIdAsync(id);
        }

        public async Task AddReviewAsync(Review review)
        {
            review.Id = Guid.NewGuid();
            review.DateCreated = DateTime.UtcNow;
            review.DateUpdated = DateTime.UtcNow;

            await _reviewRepository.AddReviewAsync(review);
        }

        public async Task UpdateReviewAsync(Review review)
        {
            review.DateUpdated = DateTime.UtcNow;

            await _reviewRepository.UpdateReviewAsync(review);
        }

        public async Task DeleteReviewAsync(Guid id)
        {
            await _reviewRepository.DeleteReviewAsync(id);
        }
    }
}
