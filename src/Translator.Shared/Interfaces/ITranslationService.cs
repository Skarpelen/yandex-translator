namespace Translator.Shared.Interfaces
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string sourceLanguage, string targetLanguage);

        string Info();
    }
}
