using System.Text.Json.Serialization;

namespace Translator.Service.Models
{
    public class Translation
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("detectedLanguageCode")]
        public string DetectedLanguageCode { get; set; }
    }
}
