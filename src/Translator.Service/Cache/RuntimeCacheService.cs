using System.Collections.Concurrent;

namespace Translator.Service.Cache
{
    public class RuntimeCacheService : ICacheService
    {
        private ConcurrentDictionary<string, string> _cachedRequests = new();

        public async Task<string?> GetAsync(string key)
        {
            return _cachedRequests.TryGetValue(key, out var value) ? value : null;
        }

        public Task SaveAsync(string key, string value)
        {
            _cachedRequests.TryAdd(key, value);
            return Task.CompletedTask;
        }

        public string Info()
        {
            return $"Cache Type: In-Memory, Item Count: {_cachedRequests.Count}";
        }
    }
}
