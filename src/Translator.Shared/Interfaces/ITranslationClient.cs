namespace Translator.Shared.Interfaces
{
    public interface ITranslationClient
    {
        Task<string> TranslateAsync(Models.TranslateRequest request);

        Task<string> GetServiceInfoAsync();
    }
}
