using Finbourne.GenericInMemoryCache.Abstractions;
using Finbourne.GenericInMemoryCache.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Finbourne.GenericInMemoryCache
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Setup.SetupDI().Build();

            var cache = host.Services.GetRequiredService<IGenericInMemoryCache>();
            cache.ItemEvicted += Cache_ItemEvicted;

            await cache.SetAsync("Key1", "value 1");
            await cache.SetAsync("Key2", "value 2");
            await cache.SetAsync("Key3", "value 3");
            await cache.SetAsync("Key4", "value 4");

            string? value1 = await cache.GetAsync<string>("key1");
            Console.ReadLine();
        }

        private static void Cache_ItemEvicted(object? sender, ICacheItemEvictedEventArgs e)
        {
            Console.WriteLine($"At client side item evicted event: Item evicted: Key={e.Key}, Value={e.Value}");
        }

    }
}
