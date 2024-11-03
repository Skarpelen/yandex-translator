using System.Text.Json.Serialization;

namespace Translator.Service.Models
{
    public class YandexTranslateResponse
    {
        [JsonPropertyName("translations")]
        public List<Translation> Translations { get; set; }
    }
}
