using Cinema.Model;

namespace Cinema.Repository.Common;

public interface IGenreRepository
{
    Task<Genre> AddGenreAsync(Genre genre);
    Task<IEnumerable<Genre>> GetAllGenresAsync();
}