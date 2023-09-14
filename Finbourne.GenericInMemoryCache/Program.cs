﻿using Finbourne.GenericInMemoryCache.Abstractions;
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
            await cache.SetCacheAsync("Key1", "value 1");
            await cache.SetCacheAsync("Key2", "value 2");
            await cache.SetCacheAsync("Key3", "value 3");
            await cache.SetCacheAsync("Key4", "value 4");

            string? value1 = await cache.GetCacheAsync<string>("key1");
            Console.WriteLine($"Value of key1 is {value1}");

            Console.ReadLine();
        }
        
    }
}
