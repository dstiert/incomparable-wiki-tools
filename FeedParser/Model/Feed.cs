using System.Collections.Generic;

namespace FeedParser.Model
{
    public class Feed
    {
        public string Title { get; set; }

        public List<Episode> Episodes { get; set;}
    }
}