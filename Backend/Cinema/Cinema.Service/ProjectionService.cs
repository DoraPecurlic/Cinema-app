using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;

namespace Cinema.Service
{
    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionRepository _projectionRepository;
        private readonly IMovieRepository _movieRepository;

        public ProjectionService(IProjectionRepository projectionRepository, IMovieRepository movieRepository)
        {
            this._projectionRepository = projectionRepository;
            this._movieRepository = movieRepository;
        }

        public async Task<List<Projection>> GetAllProjectionsAsync()
        {
            return await _projectionRepository.GetAllProjectionsAsync();
        }

        public async Task<Projection> GetProjectionByIdAsync(Guid id)
        {
            return await _projectionRepository.GetProjectionByIdAsync(id);
        }
        
        public async Task<IEnumerable<Projection>> GetProjectionsByMovieIdAsync(Guid id)
        {
            return await _projectionRepository.GetProjectionsByMovieIdAsync(id);
        }

        public async Task AddProjectionAsync(Projection projection)
        {
            projection.Id = Guid.NewGuid();
            projection.DateCreated = DateTime.UtcNow;
            projection.DateUpdated = DateTime.UtcNow;

            foreach (var projectionHall in projection.ProjectionHalls)
            {
                projectionHall.Id = Guid.NewGuid();
                projectionHall.ProjectionId = projection.Id;
            }

            await _projectionRepository.AddProjectionAsync(projection);
        }

        public async Task UpdateProjectionAsync(Projection projection)
        {
            projection.DateUpdated = DateTime.UtcNow;

            if (projection.ProjectionHalls == null)
            {
                projection.ProjectionHalls = new List<ProjectionHall>();
            }

            await _projectionRepository.UpdateProjectionAsync(projection);
        }

        public async Task DeleteProjectionAsync(Guid id)
        {
            await _projectionRepository.DeleteProjectionAsync(id);
        }
    }
}
