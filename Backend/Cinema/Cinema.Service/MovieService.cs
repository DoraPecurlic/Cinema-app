using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;
using System;
using System.Threading.Tasks;
using DTO.MovieModel;
using Cinema.Common;
using Microsoft.Extensions.Logging;

namespace Cinema.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMovieRepository movieRepository, IActorRepository actorRepository, ILogger<MovieService> logger)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _logger = logger;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            if (await _movieRepository.MovieExistsAsync(movie.Title))
            {
                throw new ArgumentException("Movie with the same title already exists.");
            }

            _logger.LogInformation("Adding movie: {@Movie}", movie);
            await _movieRepository.AddMovieAsync(movie);
        }

        public async Task AddActorToMovieAsync(Guid movieId, Guid actorId)
        {
            await _movieRepository.AddActorToMovieAsync(movieId, actorId);
        }

        public async Task<IEnumerable<MovieGet>> GetFilteredMoviesAsync(MovieFiltering filtering, MovieSorting sorting, MoviePaging paging)
        {
            return await _movieRepository.GetFilteredMoviesAsync(filtering, sorting, paging);
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var existingMovie = await _movieRepository.GetMovieByIdAsync(movie.Id);

            if (existingMovie == null)
            {
                throw new ArgumentException($"Movie with ID {movie.Id} not found.");
            }

            var updatedMovie = new Movie
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                Description = movie.Description,
                Duration = movie.Duration,
                LanguageId = movie.LanguageId,
                CoverUrl = movie.CoverUrl,
                TrailerUrl = movie.TrailerUrl,
                DateUpdated = DateTime.UtcNow,
                UpdatedByUserId = movie.UpdatedByUserId
            };

            _logger.LogInformation("Updating movie: {@Movie}", updatedMovie);
            await _movieRepository.UpdateMovieAsync(updatedMovie);
        }

        public async Task DeleteMovieAsync(Guid id)
        {
            await _movieRepository.DeleteMovieAsync(id);
        }

        public async Task DeleteActorFromMovie(Guid movieId, Guid actorId)
        {
            await _movieRepository.DeleteActorFromMovie(movieId, actorId);
        }
    }
}
