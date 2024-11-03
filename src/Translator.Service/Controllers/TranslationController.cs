using Microsoft.AspNetCore.Mvc;

namespace Translator.Service.Controllers
{
    using Translator.Shared.Interfaces;
    using Translator.Shared.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationService _translationService;

        public TranslationController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetInfo()
        {
            var info = await _translationService.GetInfoAsync();
            return Ok(info);
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            var result = await _translationService.TranslateAsync(request.Text, request.SourceLanguage, request.TargetLanguage);
            return Ok(result);
        }
    }
}
