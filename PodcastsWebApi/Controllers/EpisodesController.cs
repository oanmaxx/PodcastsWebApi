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
    public class EpisodesController : ControllerBase
    {
        private readonly PodcastsContext _context;

        public EpisodesController(PodcastsContext context)
        {
            _context = context;
        }

        // GET: api/Episodes
        /// <summary>
        /// Gets a list of all episodes of a given podcast.
        /// </summary>
        /// <param name="id">The ID of the podcast.</param>
        /// <returns>Returns a list of all episodes. Request must be made by a logged in user.</returns>
        [HttpGet("[action]/{id}")]
        public IEnumerable<Episodes> PodcastEpisodes(long id)
        {
            var episodes = _context.Episodes.Where(a => a.PodcastId == id).ToList();

            if (episodes == null)
            {
                return new List<Episodes>();
            }

            return episodes;
        }

        // GET: api/Episodes
        /// <summary>
        /// Gets a list of episodes of a podcast with a given ID.
        /// </summary>
        /// <param name="id">The ID of the podcast.</param>
        /// <param name="filter"></param>
        /// <returns>A list of episodes.</returns>
        [HttpGet("[action]/{id}/{filter}")]
        public IEnumerable<Episodes> PodcastEpisodesFilter(long id, string filter)
        {
            var episodes = _context.Episodes.Where(a => a.PodcastId == id && a.Title.Contains(filter)).ToList();

            if (episodes == null)
            {
                return new List<Episodes>();
            }

            return episodes;
        }

        // GET: api/Episodes/5
        /// <summary>
        /// Finds an episode by ID.
        /// </summary>
        /// <param name="id">The episode ID.</param>
        /// <returns>Returns an episode based on ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Episodes>> GetEpisodes(long id)
        {
            var episodes = await _context.Episodes.FindAsync(id);

            if (episodes == null)
            {
                return NotFound();
            }

            return episodes;
        }

        // PUT: api/Episodes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Updates an existing episode based on ID.
        /// </summary>
        /// <param name="id">The episode ID.</param>
        /// <param name="episodes">The episode.</param>
        /// <returns>Returns the episode.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Episodes>> PutEpisodes(long id, Episodes episodes)
        {
            if (id != episodes.Id)
            {
                return BadRequest();
            }

            _context.Entry(episodes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EpisodesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return episodes;
        }

        // POST: api/Episodes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Posts episodes.
        /// </summary>
        /// <param name="episodes">The episodes which will be added.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Episodes>> PostEpisodes(Episodes episodes)
        {
            _context.Episodes.Add(episodes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEpisodes", new { id = episodes.Id }, episodes);
        }

        // DELETE: api/Episodes/5
        /// <summary>
        /// Deletes the episode with the given ID.
        /// </summary>
        /// <param name="id">The ID of the episode.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Episodes>> DeleteEpisodes(long id)
        {
            var episodes = await _context.Episodes.FindAsync(id);
            if (episodes == null)
            {
                return NotFound();
            }

            _context.Episodes.Remove(episodes);
            await _context.SaveChangesAsync();

            return episodes;
        }

        private bool EpisodesExists(long id)
        {
            return _context.Episodes.Any(e => e.Id == id);
        }
    }
}
