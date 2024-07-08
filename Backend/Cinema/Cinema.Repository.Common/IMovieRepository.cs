using Cinema.Model;
using Cinema.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MovieModel;

namespace Cinema.Repository.Common
{
    public interface IMovieRepository
    {
       
        Task AddMovieAsync(Movie movie);
        Task AddActorToMovieAsync(Guid movieId, Guid actorId);
        Task<MovieGet> GetMovieByIdAsync(Guid id);
        Task<IEnumerable<MovieGet>> GetFilteredMoviesAsync(MovieFiltering filtering, MovieSorting sorting, MoviePaging paging);
        Task<bool> MovieExistsAsync(string title);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(Guid id);
        Task DeleteActorFromMovie(Guid movieId, Guid actorId);

    }
}
