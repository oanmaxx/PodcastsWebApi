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
        //long podcastId;
        public string Title { get; set; }
        public string Description { get; set; }

        public Podcast Podcast { get; set; }
    }
}
