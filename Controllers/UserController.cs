using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Models;
using BookStoreApi.Models.Contexts;
using Microsoft.Extensions.Logging;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BookStorePMAB _context;
        private readonly ILogger<UserController> _logger;

        public UserController(BookStorePMAB context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (_context.Users == null)
            {
                _logger.LogError("Entity set 'BookStorePMAB.Users' is null.");
                return Problem("Entity set 'BookStorePMAB.Users' is null.");
            }

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Username))
            {
                _logger.LogWarning("Invalid registration data: Email, Password, or Username is empty.");
                return BadRequest("Email, Password, and Username are required.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists.", user.Email);
                return BadRequest("Email already exists.");
            }

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User registered successfully: {Email}", user.Email);
                return Ok(new { token = user.UserId.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user: {Email}", user.Email);
                return StatusCode(500, "Internal server error during registration.");
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] User loginUser)
        {
            if (_context.Users == null)
            {
                _logger.LogError("Entity set 'BookStorePMAB.Users' is null.");
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(loginUser.Email) || string.IsNullOrWhiteSpace(loginUser.Password))
            {
                _logger.LogWarning("Invalid login data: Email or Password is empty.");
                return BadRequest("Email and Password are required.");
            }

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: Invalid credentials for {Email}.", loginUser.Email);
                    return Unauthorized("Invalid email or password.");
                }

                _logger.LogInformation("User logged in successfully: {Email}", loginUser.Email);
                return Ok(new { token = user.UserId.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user: {Email}", loginUser.Email);
                return StatusCode(500, "Internal server error during login.");
            }
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                _logger.LogError("Entity set 'BookStorePMAB.Users' is null.");
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                _logger.LogError("Entity set 'BookStorePMAB.Users' is null.");
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning("User not found: ID {Id}", id);
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                _logger.LogWarning("Invalid update attempt: ID mismatch for User ID {Id}", id);
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User updated successfully: ID {Id}", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    _logger.LogWarning("User not found for update: ID {Id}", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Concurrency error updating user: ID {Id}", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: ID {Id}", id);
                return StatusCode(500, "Internal server error during update.");
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                _logger.LogError("Entity set 'BookStorePMAB.Users' is null.");
                return Problem("Entity set 'BookStorePMAB.Users' is null.");
            }

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created successfully: {Email}", user.Email);
                return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Email}", user.Email);
                return StatusCode(500, "Internal server error during user creation.");
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                _logger.LogError("Entity set 'BookStorePMAB.Users' is null.");
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found for deletion: ID {Id}", id);
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User deleted successfully: ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: ID {Id}", id);
                return StatusCode(500, "Internal server error during deletion.");
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}