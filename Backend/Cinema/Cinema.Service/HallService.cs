using Cinema.Model;
using Cinema.Service.Common;
using Cinema.Repository.Common;
using DTO.HallModel;
using DTO.MovieModel;

namespace Cinema.Service
{
    public class HallService : IHallService
    {
        private readonly IHallRepository _hallRepository;
        private readonly IMovieRepository _movieRepository;

        public HallService(IHallRepository hallRepository, IMovieRepository movieRepository)
        {
            this._hallRepository = hallRepository;
            this._movieRepository = movieRepository;
        }

        public async Task AddHallAsync(Hall hall)
        {
            await _hallRepository.AddHallAsync(hall);
        }

        public async Task<IEnumerable<Hall>> GetAllHallsAsync()
        {
            return await _hallRepository.GetAllHallsAsync();
        }

        public async Task<Hall> GetHallByIdAsync(Guid id)
        {
            return await _hallRepository.GetHallByIdAsync(id);
        }
        
        public async Task<List<AvailableHallGet>> GetAvailableHallsAsync(DateOnly date, TimeOnly time, Guid movieId)
        {
            MovieGet movie = await _movieRepository.GetMovieByIdAsync(movieId);
            var movieDuration = movie.Duration;

            var movieDurationTimeSpan = TimeSpan.FromMinutes(movieDuration);

            var projectionLowerLimit = time.AddMinutes(-29);
            var projectionUpperLimit = time.Add(movieDurationTimeSpan).AddMinutes(29);

            return await _hallRepository.GetAvailableHallsAsync(date, projectionLowerLimit, projectionUpperLimit);
        }
        
        public async Task<Hall?> GetHallByProjectionIdAsync(Guid projectionId)
        {
            return await _hallRepository.GetHallByProjectionIdAsync(projectionId);
        }

        public async Task UpdateHallAsync(Hall hall)
        {
            await _hallRepository.UpdateHallAsync(hall);
        }

        public async Task DeleteHallAsync(Guid id)
        {
            await _hallRepository.DeleteHallAsync(id);
        }
    }
}
