using Common.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WikiClientLibrary.Client;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Sites;

namespace WikiWriter
{
    public class Wiki
    {
        public readonly WikiSite _site;

        public Wiki(WikiOptions options, ILoggerFactory logFactory)
        {
            var client = new WikiClient()
            {
                ClientUserAgent = options.UserAgent,
                Logger = logFactory.CreateLogger<WikiClient>()
            };

            _site = new WikiSite(client, options.ApiEndpoint)
            {
                Logger = logFactory.CreateLogger<WikiSite>()
            };
            _site.Initialization.Wait();
            _site.LoginAsync(options.Username, options.Password).Wait();
        }

        public async Task<bool> PageExists(string title)
        {
            var page = new WikiPage(_site, title);
            await page.RefreshAsync();
            return page.Exists;
        }

        public async Task<bool> CreatePage(string title, string summary, string content)
        {
            if(await PageExists(title))
            {
                return false;
            }

            var page = new WikiPage(_site, title);
            page.Content = content;
            await page.UpdateContentAsync(summary, false, true);
            return true;
        }
    }
}