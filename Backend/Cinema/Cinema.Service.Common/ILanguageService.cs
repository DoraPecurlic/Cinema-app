using Cinema.Model;

namespace Cinema.Service.Common;

public interface ILanguageService
{
    Task<Language> AddLanguageAsync(Language language);
    Task<IEnumerable<Language>> GetAllLanguagesAsync();
}