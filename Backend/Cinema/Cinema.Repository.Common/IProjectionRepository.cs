using Cinema.Model;

namespace Cinema.Repository.Common
{
    public interface IProjectionRepository
    {
        Task<List<Projection>> GetAllProjectionsAsync();
        Task<Projection?> GetProjectionByIdAsync(Guid id);
        Task<IEnumerable<Projection>> GetProjectionsByMovieIdAsync(Guid id);
        Task UpdateProjectionAsync(Projection projection);
        Task AddProjectionAsync(Projection projection);
        Task DeleteProjectionAsync(Guid id);
    }
}
