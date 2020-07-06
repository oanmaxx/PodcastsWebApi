using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PodcastsWebApi.Models
{
    public class Favorites
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long Podcast { get; set; }
    }
}
