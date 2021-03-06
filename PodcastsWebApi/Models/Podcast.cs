﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastsWebApi.Models
{
    public class Podcast
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
        public int NumberOfEpisodes { get; set; }
        public string Picture { get; set; } 

        public List<Episodes> Episodes { get; set; }
    }
}
