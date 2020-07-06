using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodcastsWebApi.Models;

namespace PodcastsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PodcastsController : ControllerBase
    {
        private readonly PodcastsContext _context;

        public PodcastsController(PodcastsContext context)
        {
            _context = context;
        }

        // GET: api/Podcasts
        /// <summary>
        /// Gets all podcasts.
        /// </summary>
        /// <returns>Returns a list of all podcasts.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podcast>>> GetPodcasts()
        {
            return await _context.Podcasts.ToListAsync();
        }

        // GET: api/Podcasts/filter/something
        /// <summary>
        /// Finds the podcasts based on a title filter.
        /// </summary>
        /// <param name="filter">A filter for podcast title.</param>
        /// <returns>Returns a list of Podcasts.</returns>
        [HttpGet("[action]/{filter}")]
        public async Task<ActionResult<IEnumerable<Podcast>>> Filter(string filter)
        {
            return await _context.Podcasts.Where(a=>a.Title.Contains(filter)).ToListAsync();
        }

        // GET: api/Podcasts/5
        /// <summary>
        /// Finds podcast based on ID.
        /// </summary>
        /// <param name="id">The podcast ID.</param>
        /// <returns>Returns a podcast.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Podcast>> GetPodcast(long id)
        {
            var podcast = await _context.Podcasts.FindAsync(id);

            if (podcast == null)
            {
                return NotFound();
            }

            return podcast;
        }

        // PUT: api/Podcasts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Updates an existing podcast.
        /// </summary>
        /// <param name="id">The podcast ID.</param>
        /// <param name="podcast">The podcast.</param>
        /// <returns>The updated podcast.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Podcast>> PutPodcast(long id, Podcast podcast)
        {
            if (id != podcast.Id)
            {
                return BadRequest();
            }

            _context.Entry(podcast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PodcastExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return podcast;
        }

        // POST: api/Podcasts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Adds a podcast.
        /// </summary>
        /// <param name="podcast">The given podcast.</param>
        /// <returns>returns a podcast.</returns>
        [HttpPost]
        public async Task<ActionResult<Podcast>> PostPodcast(Podcast podcast)
        {
            _context.Podcasts.Add(podcast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPodcast", new { id = podcast.Id }, podcast);
        }

        // DELETE: api/Podcasts/5
        /// <summary>
        /// Deletes a podcast with a given id.
        /// </summary>
        /// <param name="id">The podcast id.</param>
        /// <returns>The podcast that needs to be deleted.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Podcast>> DeletePodcast(long id)
        {
            var podcast = await _context.Podcasts.FindAsync(id);
            if (podcast == null)
            {
                return NotFound();
            }

            _context.Podcasts.Remove(podcast);
            await _context.SaveChangesAsync();

            return podcast;
        }

        private bool PodcastExists(long id)
        {
            return _context.Podcasts.Any(e => e.Id == id);
        }
    }
}
