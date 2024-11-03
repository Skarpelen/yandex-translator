using System.Net.Http.Json;

namespace Translator.ConsoleApp
{
    using Translator.Shared.Interfaces;
    using Translator.Shared.Models;

    public class RestTranslationClient : ITranslationClient
    {
        private readonly HttpClient _client;

        public RestTranslationClient()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://localhost:7216/api/translation/") };
        }

        public async Task<string> TranslateAsync(TranslateRequest request)
        {
            var response = await _client.PostAsJsonAsync("translate", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine("Error: " + response.ReasonPhrase);
            return null;
        }

        public async Task<string> GetServiceInfoAsync()
        {
            var response = await _client.GetAsync("info");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine("Error: " + response.ReasonPhrase);
            return null;
        }
    }
}
