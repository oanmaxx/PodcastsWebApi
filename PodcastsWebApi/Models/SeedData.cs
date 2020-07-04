using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace PodcastsWebApi.Models
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var podcastsContext = new PodcastsContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PodcastsContext>>()))
            {
                SeedUsers(podcastsContext);
                SeedPodcasts(podcastsContext);
                SeedEpisodes(podcastsContext);
            }
        }

        private static void SeedUsers(PodcastsContext podcastsContext)
        {
            // Look for any users.
            if (podcastsContext.Users.Any())
            {
                return;   // DB table(Users) has been seeded
            }

            podcastsContext.Users.AddRange(
                new User
                {
                    FirstName = "Oana",
                    LastName = "Maxim",
                    UserName = "oanmax",
                    EmailAddress = "oan.max@yahoo.com",
                    Password = "Qwerty2020",
                },

                new User
                {
                    FirstName = "Diana",
                    LastName = "Maxim",
                    UserName = "diamax",
                    EmailAddress = "dia.max@yahoo.com",
                    Password = "Qwerty2021",
                }
            );
            podcastsContext.SaveChanges();
        }

        private static void SeedPodcasts(PodcastsContext podcastsContext)
        {
            // Look for any podcasts.
            if (podcastsContext.Podcasts.Any())
            {
                return;   // DB table(Podcasts) has been seeded
            }

            podcastsContext.Podcasts.AddRange(
                new Podcast
                {
                    Title = "Greece",
                    Url = "https://en.wikipedia.org/wiki/Greece",
                    Author = "oanmax",
                    NumberOfEpisodes = 1,
                    Picture = "A picture from Greece"
                },

                new Podcast
                {
                    Title = "Summer Time",
                    Url = "https://en.wikipedia.org/wiki/Summer",
                    Author = "oanmax",
                    NumberOfEpisodes = 2,
                    Picture = "A lazy picture"
                }
            );
            podcastsContext.SaveChanges();
        }

        private static void SeedEpisodes(PodcastsContext podcastsContext)
        {
            // Look for any episodes.
            if (podcastsContext.Episodes.Any())
            {
                return;   // DB table(Episodes) has been seeded
            }

            podcastsContext.Episodes.AddRange(
                new Episodes
                {
                    Title = "Greece",
                    Description = "Fauna",
                    Podcast = podcastsContext.Podcasts.ElementAt(0)
                },
                new Episodes
                {
                    Title = "First day of Summer",
                    Description = "1st June",
                    Podcast = podcastsContext.Podcasts.ElementAt(1)
                },

                new Episodes
                {
                    Title = "Summer solstice",
                    Description = "21st June",
                    Podcast = podcastsContext.Podcasts.ElementAt(1)
                }
            );
            podcastsContext.SaveChanges();
        }
    }
}
