using FeedParser.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System;
using Microsoft.Extensions.Logging;

namespace FeedParser
{
    public class Parser
    {
        private readonly ILogger<Parser> _logger;

        public Parser(ILogger<Parser> logger)
        {
            _logger = logger;
        }

        public Feed ParseFeed(string url)
        {
            var feedXml = new XmlDocument();
            feedXml.Load(url);
            var rss = feedXml["rss"]["channel"];

            if(rss != null)
            {
                var feed = new Feed();
                feed.Title = rss["title"].InnerText;
                feed.Episodes = rss.GetElementsByTagName("item").Cast<XmlElement>().Select(i => ParseEpisode(i)).ToList();

                for(int i = 1; i < feed.Episodes.Count - 1; i++)
                {
                    feed.Episodes[i + 1].Next = feed.Episodes[i].Title;
                    feed.Episodes[i - 1].Previous = feed.Episodes[i].Title;
                }

                return feed;
            }

            return null;
        }

        private Episode ParseEpisode(XmlElement xml)
        {
            var episode = new Episode
            {
                Title = xml["itunes:title"]?.InnerText ?? xml["title"]?.InnerText,
                SubTitle = xml["itunes:subtitle"]?.InnerText,
                Image = xml["itunes:image"]?.InnerText,
                Duration = xml["itunes:duration"]?.InnerText,
                Explicit = xml["itunes:explicit"]?.InnerText?.Equals("yes") ?? false,
                Link = xml["link"]?.InnerText,
                Description = xml["description"]?.InnerText
            };

            DateTime pub;
            if(DateTime.TryParse(xml["pubDate"]?.InnerText, out pub))
            {
                episode.Published = pub;
            }

            episode.EpisodeNumber = xml["itunes:episode"]?.InnerText ?? EpisodeNumber(xml["title"]?.InnerText);
            var content = xml["content:encoded"]?.InnerText;

            if(content != null)
            {
                episode.Panelists = ParseLinks(content, "<a href=\"https:\\/\\/www.theincomparable.com\\/person\\/.+?>(.+?)<\\/a>");
                episode.Works = ParseLinks(content, "<a href=\"https:\\/\\/www.theincomparable.com\\/work\\/.+?>(.+?)<\\/a>");
            }

            return episode;
        }

        private List<string> ParseLinks(string document, string regex)
        {
            return Regex.Matches(document, regex).Cast<Match>().Select(m => m.Groups[1].Value).ToList();
        }

        private string EpisodeNumber(string title)
        {
            if(title == null)
            {
                return null;
            }
            return title.Split(':')[0];
        }
    }
}