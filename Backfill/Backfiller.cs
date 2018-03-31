using System.Collections.Generic;
using Common.Configuration;
using Common.Templates;
using FeedParser;
using Microsoft.Extensions.Logging;
using WikiWriter;

namespace Backfill
{
    public class Backfiller
    {
        private readonly ILogger<Backfiller> _logger;
        private readonly Parser _parser;
        private readonly Wiki _wiki;
        private readonly List<string> _feeds;

        public Backfiller(ILogger<Backfiller> logger, Parser parser, Wiki wiki, FeedOptions options)
        {
            _logger = logger;
            _parser = parser;
            _wiki = wiki;
            _feeds = options.Feeds;
        }

        public void Run()
        {
            foreach(var url in _feeds)
            {
                var feed =  _parser.ParseFeed(url);
                _logger.LogInformation("Parsed {0} feed", feed.Title);
                _logger.LogInformation(EpisodePage.Template(feed.Episodes[0]));
            }
        }
    }
}