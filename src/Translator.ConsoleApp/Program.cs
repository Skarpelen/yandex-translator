using System.Net.Http.Json;

namespace Translator.ConsoleApp
{
    using Translator.Shared.Models;

    public class Program
    {
        private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:7216/api/translation/") };

        static async Task Main(string[] args)
        {
            Console.WriteLine("Translation Console App");
            Console.Write("Enter text to translate: ");
            var text = Console.ReadLine();

            Console.Write("Enter target language (e.g., 'ru' for Russian): ");
            var targetLanguage = Console.ReadLine();

            var request = new TranslateRequest
            {
                Text = text,
                TargetLanguage = targetLanguage
            };

            var translation = await TranslateAsync(request);
            Console.WriteLine("Translation: " + translation);

            var info = await GetServiceInfoAsync();
            Console.WriteLine("Service Info: " + info);

            Console.ReadLine();
        }

        static async Task<string> TranslateAsync(TranslateRequest request)
        {
            var response = await client.PostAsJsonAsync("translate", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine("Error: " + response.ReasonPhrase);
            return null;
        }

        static async Task<string> GetServiceInfoAsync()
        {
            var response = await client.GetAsync("info");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine("Error: " + response.ReasonPhrase);
            return null;
        }
    }
}
