using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastsWebApi.Models
{
    public class Episodes
    {
        public long Id { get; set; }
        public long PodcastId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }

        public string PubDate { get; set; }

        public string Link { get; set; }

        public Podcast Podcast { get; set; }
    }
}
