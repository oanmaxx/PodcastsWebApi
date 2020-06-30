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
            using (var usercontext = new UserContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UserContext>>()))
            {
                SeedUsers(usercontext);
            }

            using (var podcastscontext = new PodcastContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<PodcastContext>>()))
            {
                SeedPodcasts(podcastscontext);
            }
        }

        private static void SeedUsers(UserContext usercontext)
        {
            // Look for any users.
            if (usercontext.Users.Any())
            {
                return;   // DB table(Users) has been seeded
            }

            usercontext.Users.AddRange(
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
            usercontext.SaveChanges();
        }

        private static void SeedPodcasts(PodcastContext podcastscontext)
        {
            // Look for any users.
            if (podcastscontext.Podcasts.Any())
            {
                return;   // DB table(Users) has been seeded
            }

            podcastscontext.Podcasts.AddRange(
                new Podcast
                {
                    Title = "Grrece",
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
            podcastscontext.SaveChanges();
        }

        private static void SeedEpisodes(PodcastContext episodescontext)
        {
            // Look for any users.
            if (episodescontext.Podcasts.Any())
            {
                return;   // DB table(Users) has been seeded
            }

            episodescontext.Podcasts.AddRange(
                new Podcast
                {
                    Title = "Grrece",
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
            episodescontext.SaveChanges();
        }
    }
}
