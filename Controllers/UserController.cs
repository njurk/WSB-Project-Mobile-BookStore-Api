using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Model.Entities;
using BookStoreApi.Models.DTOs;
using BookStoreApi.Model.Contexts;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public UserController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            return await _context.User.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
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

        // PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserPartialUpdateDto dto)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            if (dto.Username != null) user.Username = dto.Username;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.Password != null)
            if (dto.Street != null) user.Street = dto.Street;
            if (dto.City != null) user.City = dto.City;
            if (dto.PostalCode != null) user.PostalCode = dto.PostalCode;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound();

            var carts = await _context.Cart.Where(c => c.UserId == id).ToListAsync();
            if (carts.Any())
                _context.Cart.RemoveRange(carts);

            var orders = await _context.Order.Where(o => o.UserId == id).ToListAsync();
            if (orders.Any())
                _context.Order.RemoveRange(orders);

            var reviews = await _context.Review.Where(r => r.UserId == id).ToListAsync();
            if (reviews.Any())
                _context.Review.RemoveRange(reviews);

            var collections = await _context.Collection.Where(c => c.UserId == id).ToListAsync();
            if (collections.Any())
                _context.Collection.RemoveRange(collections);

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u =>
                    (u.Email == request.Identifier || u.Username == request.Identifier)
                    && u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid username/email or password");

            return Ok(user);
        }
    }
}
