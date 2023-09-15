using Microsoft.Extensions.Logging;
using Moq;


namespace Finbourne.GenericInMemoryCache.Test
{
    public class InMemoryCacheTests
    {
        private ILogger<InMemoryCache> logger;
        private CacheConfiguration cacheConfiguration;

        [SetUp]
        public void Setup()
        {
            logger =  new Mock<ILogger<InMemoryCache>>().Object;
            cacheConfiguration = new CacheConfiguration { MaxCacheSize = 3 };
         }

        [Test]
        public void ShouldAddItemToCache()
        {         
            var cache = new InMemoryCache(cacheConfiguration, logger);
            cache.SetCacheAsync("Key1", 1).Wait();
            Assert.That(cache.GetCacheAsync<int?>("Key1").Result, Is.EqualTo(1));
            cache.SetCacheAsync("Key1", "value 1").Wait();
            Assert.That(cache.GetCacheAsync<string>("Key1").Result, Is.EqualTo("value 1"));
        }
        [Test]
        public void ShouldRemoveLRU_WhenMaxCacheCapacityReached()
        {
            var cache = new InMemoryCache(cacheConfiguration, logger);
            cache.SetCacheAsync("Key1", 1).Wait();
            cache.SetCacheAsync("Key2", 2).Wait();
            cache.SetCacheAsync("Key3", 3).Wait();
            cache.SetCacheAsync("Key4", 4).Wait();

            Assert.That(cache.GetCacheAsync<int?>("Key1").Result.Equals(null));
            Assert.That(cache.GetCacheAsync<int?>("Key2").Result.Equals(2));
            Assert.That(cache.GetCacheAsync<int?>("Key3").Result.Equals(3));
            Assert.That(cache.GetCacheAsync<int?>("Key4").Result.Equals(4));
        }
        [Test]
        public void ShouldRemoveItemFromCache()
        {
            var cache = new InMemoryCache(cacheConfiguration, logger);
            cache.SetCacheAsync("Key1", 1).Wait();
            cache.SetCacheAsync("Key2", 2).Wait();
            cache.SetCacheAsync("Key3", 3).Wait();

            cache.DeleteCacheAsync("Key2").Wait();

            Assert.That(cache.GetCacheAsync<int?>("Key2").Result.Equals(null));
        }
        [Test]
        public void ShouldClearCache()
        {
            var cache = new InMemoryCache(cacheConfiguration, logger);
            cache.SetCacheAsync("Key1", 1).Wait();
            cache.SetCacheAsync("Key2", 2).Wait();
            cache.PurgeCacheAsync().Wait();

            Assert.That(cache.GetCacheAsync<int?>("Key1").Result.Equals(null));
            Assert.That(cache.GetCacheAsync<int?>("Key2").Result.Equals(null));
        }

        [Test]
        public void ShouldReturnNull_WhenGetItemFromCache()
        {
            var cache = new InMemoryCache(cacheConfiguration, logger);
            Assert.That(cache.GetCacheAsync<string>("NonExistentKey").Result, Is.Null);
        }

        [Test]
        public void ShouldThrowExceptionWhenInvalidConfiguration()
        {
            Assert.Throws<ArgumentException>(() => new InMemoryCache(new CacheConfiguration{ MaxCacheSize = 0 }, logger));
        }

        [Test]
        public void ShouldCallItemEvictedEvent()
        {
            var cache = new InMemoryCache(cacheConfiguration, logger);
            
            bool ensure_event_called = false;
            
            cache.ItemEvicted += (sender, args) => {
                ensure_event_called = true;

            };

            cache.SetCacheAsync("key1", "value1");
            cache.DeleteCacheAsync("key1");

            Assert.That(ensure_event_called, Is.True);

            ensure_event_called = false;

            cache.SetCacheAsync("key1", "value1");
            cache.SetCacheAsync("key2", "value2");
            cache.SetCacheAsync("key3", "value3");

            Assert.That(ensure_event_called, Is.False);

            cache.SetCacheAsync("key4", "value4");

            Assert.That(ensure_event_called, Is.True);

        }
    }
}
