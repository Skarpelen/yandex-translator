using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Translator.Service.Services
{
    using Translator.Service.Cache;
    using Translator.Service.Models;
    using Translator.Service.Structs;
    using Translator.Shared.Interfaces;

    public class YandexTranslationService : ITranslationService
    {
        private readonly ICacheService _cacheService;
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;
        private readonly YandexConfiguration _config;

        private List<string> _supportedLanguages;

        public YandexTranslationService(ICacheService cacheService, TokenService tokenService, IOptions<YandexConfiguration> config)
        {
            _cacheService = cacheService;
            _tokenService = tokenService;
            _config = config.Value;
            _httpClient = new HttpClient();

            _supportedLanguages = GetSupportedLanguagesAsync().Result;
        }

        public string Info()
        {
            return $"Yandex Translate API.\n{_cacheService.Info()}";
        }

        public async Task<string> TranslateAsync(string text, string targetLanguage)
        {
            var loweredLanguageCode = targetLanguage.ToLower();

            if (!_supportedLanguages.Contains(loweredLanguageCode))
            {
                return $"The language code {targetLanguage} could not be recognized";
            }

            var key = $"{loweredLanguageCode}:{text}";
            var cachedTranslation = await _cacheService.GetAsync(key);

            if (!string.IsNullOrEmpty(cachedTranslation))
            {
                return cachedTranslation;
            }

            var requestBody = new YandexTranslateRequest
            {
                TargetLanguageCode = loweredLanguageCode,
                Texts = new[] { text },
                FolderId = _config.FolderName
            };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(
                "https://translate.api.cloud.yandex.net/translate/v2/translate",
                requestContent
            );

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var translateResponse = JsonSerializer.Deserialize<YandexTranslateResponse>(responseBody);

                var translatedText = translateResponse?.Translations?[0]?.Text;

                if (!string.IsNullOrEmpty(translatedText))
                {
                    await _cacheService.SaveAsync(key, translatedText);
                    return translatedText;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Translation request failed. Status: {response.StatusCode}, Content: {errorContent}");
            }

            throw new HttpRequestException("Translation request failed.");
        }

        public async Task<List<string>> GetSupportedLanguagesAsync()
        {
            if (_supportedLanguages != null)
            {
                return _supportedLanguages;
            }

            var requestBody = new { folderId = _config.FolderName };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var token = await _tokenService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(
                "https://translate.api.cloud.yandex.net/translate/v2/languages",
                requestContent
            );

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var languagesResponse = JsonSerializer.Deserialize<YandexSupportedLanguagesResponse>(responseBody);
                _supportedLanguages = languagesResponse?.Languages.Select(l => l.Code).ToList() ?? new List<string>();
                return _supportedLanguages;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to get supported languages. Status: {response.StatusCode}, Content: {errorContent}");
            }
        }
    }
}