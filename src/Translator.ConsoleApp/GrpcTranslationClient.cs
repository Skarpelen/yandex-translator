using Grpc.Net.Client;

namespace Translator.ConsoleApp
{
    using Translator.Shared.Interfaces;
    using Translator.Shared.Models;

    public class GrpcTranslationClient : ITranslationClient
    {
        private readonly Shared.Translator.TranslatorClient _client;

        public GrpcTranslationClient(string serverAddress)
        {
            var channel = GrpcChannel.ForAddress(serverAddress);
            _client = new Shared.Translator.TranslatorClient(channel);
        }

        public async Task<string> TranslateAsync(TranslateRequest request)
        {
            var grpcRequest = new Shared.TranslateRequest
            {
                Text = request.Text,
                TargetLanguage = request.TargetLanguage,
                SourceLanguage = "auto"
            };

            var reply = await _client.TranslateAsync(grpcRequest);
            return reply.TranslatedText;
        }

        public async Task<string> GetServiceInfoAsync()
        {
            var reply = await _client.GetInfoAsync(new Shared.InfoRequest());
            return reply.Info;
        }
    }
}
