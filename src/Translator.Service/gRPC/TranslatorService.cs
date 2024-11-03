using Grpc.Core;

namespace Translator.Service.gRPC
{
    using Translator.Shared;
    using Translator.Shared.Interfaces;

    public class TranslatorService : Translator.TranslatorBase
    {
        private readonly ITranslationService _translationService;

        public TranslatorService(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        public override async Task<TranslateReply> Translate(TranslateRequest request, ServerCallContext context)
        {
            var translation = await _translationService.TranslateAsync(request.Text, request.SourceLanguage, request.TargetLanguage);
            return new TranslateReply { TranslatedText = translation };
        }

        public override async Task<InfoReply> GetInfo(InfoRequest request, ServerCallContext context)
        {
            var info = await _translationService.GetInfoAsync();
            return new InfoReply { Info = info };
        }
    }
}
