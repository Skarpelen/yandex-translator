using System.Text.Json.Serialization;

namespace Translator.Service.Models
{
    public class YandexSupportedLanguagesResponse
    {
        [JsonPropertyName("languages")]
        public List<Language> Languages { get; set; }
    }
}
