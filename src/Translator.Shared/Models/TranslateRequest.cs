namespace Translator.Shared.Models
{
    public class TranslateRequest
    {
        public string SourceLanguage { get; set; }

        public string TargetLanguage { get; set; }

        public string Text { get; set; }
    }
}
