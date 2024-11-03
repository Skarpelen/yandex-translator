namespace Translator.ConsoleApp
{
    using Translator.Shared.Interfaces;
    using Translator.Shared.Models;

    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Translation Console App");
            Console.WriteLine("Choose connection type:");
            Console.WriteLine("1 - REST API");
            Console.WriteLine("2 - gRPC");

            ITranslationClient client = null;
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    client = new RestTranslationClient();
                    break;
                case "2":
                    client = new GrpcTranslationClient("https://localhost:7216");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting...");
                    Console.ReadKey();
                    return;
            }

            Console.Write("Enter text to translate: ");
            var text = Console.ReadLine();

            Console.Write("Enter target language (e.g., 'ru' for Russian): ");
            var targetLanguage = Console.ReadLine();

            var request = new TranslateRequest
            {
                Text = text,
                SourceLanguage = "auto",
                TargetLanguage = targetLanguage
            };

            var translation = await client.TranslateAsync(request);
            Console.WriteLine("Translation: " + translation);

            var info = await client.GetServiceInfoAsync();
            Console.WriteLine("Service Info: " + info);

            Console.ReadKey();
        }
    }
}