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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podcast>>> GetPodcasts()
        {
            return await _context.Podcasts.ToListAsync();
        }

        // GET: api/Podcasts/5
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPodcast(long id, Podcast podcast)
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

            return NoContent();
        }

        // POST: api/Podcasts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Podcast>> PostPodcast(Podcast podcast)
        {
            _context.Podcasts.Add(podcast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPodcast", new { id = podcast.Id }, podcast);
        }

        // DELETE: api/Podcasts/5
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
