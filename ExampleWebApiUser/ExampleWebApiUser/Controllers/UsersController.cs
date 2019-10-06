using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExampleWebApiUser.Models;

namespace ExampleWebApiUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Inject the dbContext
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Users
        /// 
        /// Returns all the users
        /// 
        /// </summary>
        [HttpGet]
        public IEnumerable<User> GetUserItems()
        {
            return _context.UserItems.Include(u => u.Address);
        }

        /// <summary>
        /// GET: api/Users/{id}
        /// 
        /// Returns all Users
        /// 
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.UserItems.Include(u => u.Address).Where(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// PUT: api/Users/{id}
        /// 
        /// Modify User
        /// 
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="user">User modified</param>        
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyUser([FromRoute] long id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            var userDb = await _context.UserItems.FindAsync(id);
            if (userDb == null)
            {
                return NotFound();
            }

            var addressDb = userDb.Address;
            userDb.Name = user.Name;
            userDb.LastName = user.LastName;
            userDb.UpdateDate = DateTime.UtcNow;

            userDb.Address = new Address();
            userDb.Address.Street = user.Address.Street;
            userDb.Address.Number = user.Address.Number;
            userDb.Address.Region = user.Address.Region;
            userDb.Address.Country = user.Address.Country;

            _context.Entry(userDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// POST: api/Users
        /// 
        /// Create user
        /// 
        /// </summary>
        /// <param name="user">User modified</param>
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.CreateDate = DateTime.UtcNow;
            _context.UserItems.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// DELETE: api/Users/{id}
        /// 
        /// Delete user
        /// 
        /// </summary>
        /// <param name="id">Id of User</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.UserItems.FindAsync(id);
            if (!UserExists(id))
            {
                return NotFound();
            }

            _context.UserItems.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(long id)
        {
            return _context.UserItems.Any(e => e.Id == id);
        }
    }
}