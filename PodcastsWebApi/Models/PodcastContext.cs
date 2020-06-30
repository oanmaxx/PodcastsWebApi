using Microsoft.EntityFrameworkCore;

namespace PodcastsWebApi.Models
{
    public class PodcastContext : DbContext
    {
        public PodcastContext(DbContextOptions<PodcastContext> options)
            :base(options)
        {
        }

        public DbSet<Podcast> Podcasts { get; set; }
    }
}
