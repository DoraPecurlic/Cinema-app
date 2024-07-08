using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;

namespace Cinema.Service;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Genre> AddGenreAsync(Genre genre)
    {
        genre.Id = Guid.NewGuid();
        return await _genreRepository.AddGenreAsync(genre);
    }
    
    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await _genreRepository.GetAllGenresAsync();
    }
}