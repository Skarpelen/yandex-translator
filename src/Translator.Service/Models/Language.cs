using System.Text.Json.Serialization;

namespace Translator.Service.Models
{
    public class Language
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
