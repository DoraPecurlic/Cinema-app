using Cinema.Model;

namespace Cinema.Service.Common
{
    public interface IProjectionService
    {

        Task<List<Projection>> GetAllProjectionsAsync();
        Task<Projection> GetProjectionByIdAsync(Guid id);
        Task<IEnumerable<Projection>> GetProjectionsByMovieIdAsync(Guid id);
        Task AddProjectionAsync(Projection projection);
        Task UpdateProjectionAsync(Projection projection);
        Task DeleteProjectionAsync(Guid id);
    }
}
