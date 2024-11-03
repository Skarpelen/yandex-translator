namespace Translator.Service.Cache
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string key);

        Task SaveAsync(string key, string value);

        string Info();
    }
}
