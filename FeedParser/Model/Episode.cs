using System.Collections.Generic;
using System;

namespace FeedParser.Model
{
    public class Episode
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Image { get; set;}
        
        public string EpisodeNumber { get; set; }

        public string Duration { get; set;}

        public bool Explicit { get; set;}

        public string Link { get; set; }

        public string Description { get; set; }

        public List<string> Panelists { get; set;}

        public List<string> Works { get; set;}

        public DateTime? Published { get; set; }

        public string Next { get; set; }

        public string Previous { get; set;}
    }
}