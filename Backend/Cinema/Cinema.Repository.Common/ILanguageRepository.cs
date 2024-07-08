using Cinema.Model;

namespace Cinema.Repository.Common;

public interface ILanguageRepository
{
    Task<Language> AddLanguageAsync(Language language);
    Task<IEnumerable<Language>> GetAllLanguagesAsync();
}