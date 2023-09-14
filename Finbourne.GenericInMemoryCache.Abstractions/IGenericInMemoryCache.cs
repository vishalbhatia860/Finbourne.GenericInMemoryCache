namespace Finbourne.GenericInMemoryCache.Abstractions
{
    public interface IGenericInMemoryCache
    {
        public Task<T?> GetCacheAsync<T>(string key);

        public Task SetCacheAsync<T>(string key, T value);

        public Task<bool> DeleteCacheAsync(string key);

        public Task PurgeCacheAsync();

    }
}