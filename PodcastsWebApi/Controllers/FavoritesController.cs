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
    public class FavoritesController : ControllerBase
    {
        private readonly PodcastsContext _context;

        public FavoritesController(PodcastsContext context)
        {
            _context = context;
        }

        // GET: api/Favorites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorites>>> GetFavorites()
        {
            return await _context.Favorites.ToListAsync();
        }

        // GET: api/Favorites/5
        [HttpGet("{email}")]
        public async Task<IEnumerable<Favorites>> GetFavorites(string email)
        {
            var favorites = await _context.Favorites.Where(f => f.Email == email).ToListAsync();

            if (favorites == null)
            {
                return new List<Favorites>();
            }

            return favorites;
        }

        // PUT: api/Favorites/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<Favorites>> PutFavorites(long id, Favorites favorite)
        {
            if (id != favorite.Id)
            {
                return BadRequest();
            }

            _context.Entry(favorite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return favorite;
        }

        // POST: api/Favorites
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Podcast>> PostFavorites(Favorites favorite)
        {
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavorites", new { id = favorite.Id }, favorite);
        }

        // DELETE: api/Podcasts/5
        [HttpDelete("{email}/{podcastid}")]
        public async Task<Favorites> DeleteFavorites(string email, long podcastid)
        {
            var favorites = await _context.Favorites.Where(f=>f.Email == email && f.Podcast == podcastid).ToListAsync();
            if (favorites == null)
            {
                return null;
            }

            foreach(var fav in favorites)
            {
                _context.Favorites.Remove(fav);
            }
            
            await _context.SaveChangesAsync();

            return favorites.FirstOrDefault();
        }

        private bool FavoriteExists(long id)
        {
            return _context.Favorites.Any(e => e.Id == id);
        }
    }
}
