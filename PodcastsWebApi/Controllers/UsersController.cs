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
    public class UsersController : ControllerBase
    {
        private readonly PodcastsContext _context;

        public UsersController(PodcastsContext context)
        {
            _context = context;
        }

        // GET: api/Users
        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns>Returns all users in a list.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        /// <summary>
        /// Find user based on ID.
        /// </summary>
        /// <param name="id">The user ID></param>
        /// <returns>Returns the user with the given ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/oan.max@gmail.com/pass
        /// <summary>
        /// Gets the user with the given email and password.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The email password of the given user..</param>
        /// <returns>Returns the user with the specified email and password.</returns>
        [HttpGet("[action]/{email}/{password}")]
        public User LoginUser(string email, string password)
        {
            var user = _context.Users.Where(e => e.EmailAddress == email && e.Password == password).FirstOrDefault();

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Updates a user based on ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="user">The user that needs to pe updated.</param>
        /// <returns>Returns a succes status response with no body.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Adds a user.
        /// </summary>
        /// <param name="user">The user that needs to be added.</param>
        /// <returns>Returns a user.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Deletes a user based on ID.
        /// </summary>
        /// <param name="id">The user ID that must be deleted.</param>
        /// <returns>Returns the user that must be deleted.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
