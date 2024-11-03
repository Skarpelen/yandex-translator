using Jose;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Translator.Service.Services
{
    using Translator.Service.Models;
    using Translator.Service.Structs;

    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly YandexConfiguration _config;
        private string _iamToken;
        private DateTime _tokenExpiry;

        public TokenService(IOptions<YandexConfiguration> config)
        {
            _config = config.Value;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_iamToken) || DateTime.UtcNow >= _tokenExpiry)
            {
                await UpdateIamTokenAsync();
            }

            return _iamToken;
        }

        private async Task UpdateIamTokenAsync()
        {
            var jwtToken = CreateJwtToken();

            var requestContent = new StringContent(
                JsonSerializer.Serialize(new { jwt = jwtToken }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://iam.api.cloud.yandex.net/iam/v1/tokens", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<YandexTokenResponse>(responseBody);

                _iamToken = tokenResponse?.IamToken;
                _tokenExpiry = DateTime.UtcNow.AddHours(1); // Обновляем время истечения
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to get IAM token. Status: {response.StatusCode}, Content: {errorContent}");
            }
        }

        private string CreateJwtToken()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var headers = new Dictionary<string, object>
            {
                { "kid", _config.KeyId }
            };

            var payload = new Dictionary<string, object>
            {
                { "aud", "https://iam.api.cloud.yandex.net/iam/v1/tokens" },
                { "iss", _config.ServiceAccountId },
                { "iat", now },
                { "exp", now + 3600 }
            };

            using var rsa = RSA.Create();
            rsa.ImportFromPem(_config.PrivateKey.ToCharArray());

            return JWT.Encode(payload, rsa, JwsAlgorithm.PS256, headers);
        }
    }
}
