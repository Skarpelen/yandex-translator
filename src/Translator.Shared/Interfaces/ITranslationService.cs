namespace Translator.Shared.Interfaces
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string targetLanguage);

        string Info();
    }
}
