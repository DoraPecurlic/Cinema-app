using Cinema.Model;
using Cinema.Service.Common;
using Microsoft.AspNetCore.Mvc;


namespace Cinema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLanguageAsync([FromBody] Language language)
        {
            try
            {
                var newLanguage = await _languageService.AddLanguageAsync(language);
             
                return Ok(newLanguage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            try
            {
                var languages = await _languageService.GetAllLanguagesAsync();
             
                return Ok(languages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}

