using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;

namespace Cinema.Service;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;

    public LanguageService(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<Language> AddLanguageAsync(Language language)
    {
        language.Id = Guid.NewGuid();
        return await _languageRepository.AddLanguageAsync(language);
    }
    
    public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
    {
        return await _languageRepository.GetAllLanguagesAsync();
    }
}