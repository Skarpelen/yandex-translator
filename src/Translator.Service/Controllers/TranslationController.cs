using Microsoft.AspNetCore.Mvc;

namespace Translator.Service.Controllers
{
    using Translator.Shared.Interfaces;
    using Translator.Shared.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        private ITranslationService _translationService;

        public TranslationController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            var info = _translationService.Info();
            return Ok(info);
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text) || string.IsNullOrWhiteSpace(request.TargetLanguage))
            {
                return BadRequest("Text and target language are required.");
            }

            var result = await _translationService.TranslateAsync(request.Text, request.SourceLanguage, request.TargetLanguage);
            return Ok(result);
        }
    }
}
