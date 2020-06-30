using Microsoft.EntityFrameworkCore;

namespace PodcastsWebApi.Models
{
    public class EpisodesContext : DbContext
    {
        public EpisodesContext(DbContextOptions<EpisodesContext> options) 
            : base(options)
        {
        }

        public DbSet<Episodes> Episodes { get; set; }
    }
}
