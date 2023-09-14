using Finbourne.GenericInMemoryCache.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.GenericInMemoryCache.Client
{
    internal class Setup
    {
        public static IHostBuilder SetupDI()
        {
            var factory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });

            var builder = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
             {
                 services.AddSingleton(new CacheConfiguration { MaxCacheSize = 3 });

                 services.AddSingleton<IGenericInMemoryCache, InMemoryCache>();
                 services.AddLogging();

             });

            return builder;
        }
        
    }
}
