using Cinema.Model;

namespace Cinema.Repository.Common
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(Guid id);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(Guid id);
    }
}
