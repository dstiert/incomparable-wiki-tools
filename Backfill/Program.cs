using Common.Configuration;
using FeedParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WikiWriter;

namespace Backfill
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();

            var wikiOptions = new WikiOptions(); 
            config.GetSection("wiki").Bind(wikiOptions);
            var feedOptions = new FeedOptions(); 
            config.GetSection("feed").Bind(feedOptions);

            var services = new ServiceCollection()
                            .AddLogging(c => {
                                c.AddConsole();
                            })
                            .AddSingleton(feedOptions)
                            .AddSingleton(wikiOptions)
                            .AddSingleton<Parser>()
                            .AddSingleton<Wiki>()
                            .AddSingleton<Backfiller>()
                            .BuildServiceProvider();
            
            services.GetService<Backfiller>().Run();
        }
    }
}
