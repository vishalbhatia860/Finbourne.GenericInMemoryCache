using System.Collections.Concurrent;
using Finbourne.GenericInMemoryCache.Abstractions;
using Microsoft.Extensions.Logging;

namespace Finbourne.GenericInMemoryCache
{
    public class InMemoryCache : IGenericInMemoryCache
    {
        private readonly ILogger<InMemoryCache> logger;
        private readonly ConcurrentDictionary<string, object> _cache;
        private readonly LinkedList<string> _lruCache;
        private readonly CacheConfiguration cacheConfiguration;
        private readonly object _lock = new();
        // Event to notify when an item is evicted
        public event EventHandler<ICacheItemEvictedEventArgs>? ItemEvicted = null;

        public InMemoryCache(CacheConfiguration config, ILogger<InMemoryCache> logger)
        {
            if (config.MaxCacheSize <= 0) throw new ArgumentException("CacheConfiguration.MaxCacheSize should be greater than 0");

            this.cacheConfiguration = config;
            this.logger = logger;
            _cache = new ConcurrentDictionary<string, object>();
            _lruCache = new LinkedList<string>();
        }

        public Task SetAsync<T>(string key, T value)
        {
            lock (_lock)
            {
                if (_cache.Count >= this.cacheConfiguration.MaxCacheSize && !_cache.ContainsKey(key))
                {
                    var lruKey = _lruCache.Last?.Value;
                    logger.LogInformation($"Max cache size exceeded. Removing least recently used key: {lruKey}.");
                    if (!string.IsNullOrEmpty(lruKey))
                    {
                        DeleteAsync(lruKey).Wait();
                    }
                }
                _cache.AddOrUpdate(key, value, (_, _) => value);
                _lruCache.AddFirst(key);
            }

            logger.LogInformation("Key {key} added to cache", key);
            return Task.CompletedTask;
        }
        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var _value))
            {
                logger.LogInformation($"Key {key} found in cache.");
                T? cache_value = (T)_value;
                lock (_lock)
                {
                    _lruCache.Remove(key);
                    _lruCache.AddFirst(key);
                }
                return Task.FromResult<T?>(cache_value);
            }
            else
            {
                logger.LogInformation($"Key {key} not found in cache.");

                return Task.FromResult(default(T?));

            }

        }

        public Task<bool> DeleteAsync(string key)
        {
            bool is_deleted = _cache.TryRemove(key, out var removed_val);
            if (is_deleted)
            {
                lock (_lock)
                {
                    _lruCache.Remove(key);
                }
                logger.LogInformation($"key deleted from cache: {key}");
                OnItemEvicted(key, removed_val);
            }
            else
            {
                logger.LogInformation($"key {key} was not found in cache.");
            }
            return Task.FromResult(is_deleted);
        }

        public Task PurgeAsync()
        {
            lock (_lock)
            {
                _lruCache.Clear();
                _cache.Clear();
                logger.LogInformation($"Purged cache: Count: {_lruCache.Count}");
            }
            return Task.CompletedTask;
        }

        private Task OnItemEvicted(object key, object value)
        {
            ItemEvicted?.Invoke(this, new CacheItemEvictedEventArgs(key, value));
            return Task.CompletedTask;
        }

    }
}
