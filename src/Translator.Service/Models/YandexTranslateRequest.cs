using System.Text.Json.Serialization;

namespace Translator.Service.Models
{
    public class YandexTranslateRequest
    {
        [JsonPropertyName("folderId")]
        public string FolderId { get; set; }

        [JsonPropertyName("texts")]
        public string[] Texts { get; set; }

        [JsonPropertyName("targetLanguageCode")]
        public string TargetLanguageCode { get; set; }
    }
}
