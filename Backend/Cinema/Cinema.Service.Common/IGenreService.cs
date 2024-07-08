using Cinema.Model;

namespace Cinema.Service.Common;

public interface IGenreService
{
    Task<Genre> AddGenreAsync(Genre genre);
    Task<IEnumerable<Genre>> GetAllGenresAsync();
}