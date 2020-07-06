using Microsoft.EntityFrameworkCore;

namespace PodcastsWebApi.Models
{
    public class PodcastsContext : DbContext
    {
        public PodcastsContext(DbContextOptions<PodcastsContext> options)
            :base(options)
        {
        }

        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Episodes> Episodes { get; set; }

        public DbSet<Favorites> Favorites { get; set; }
    }
}
