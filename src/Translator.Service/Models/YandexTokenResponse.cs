using System.Text.Json.Serialization;

namespace Translator.Service.Models
{
    public class YandexTokenResponse
    {
        [JsonPropertyName("iamToken")]
        public string IamToken { get; set; }

        [JsonPropertyName("expiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}
