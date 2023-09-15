namespace Finbourne.GenericInMemoryCache.Abstractions
{
    public interface IGenericInMemoryCache
    {
        public Task<T?> GetAsync<T>(string key);

        public Task SetAsync<T>(string key, T value);

        public Task<bool> DeleteAsync(string key);

        public Task PurgeAsync();

        public event EventHandler<ICacheItemEvictedEventArgs> ItemEvicted;

    }
}
