using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Model.Entities;
using BookStoreApi.Model.Contexts;
using BookStoreApi.Models.DTOs;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public CollectionController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/Collection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollections([FromQuery] int? userId, [FromQuery] int? bookId)
        {
            if (_context.Collection == null)
                return NotFound();

            var query = _context.Collection.AsQueryable();

            if (userId.HasValue)
                query = query.Where(c => c.UserId == userId.Value);

            if (bookId.HasValue)
                query = query.Where(c => c.BookId == bookId.Value);

            query = query.Include(c => c.Book);

            var collections = await query.ToListAsync();

            return collections;
        }

        // GET: api/Collection/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Collection>> GetCollection(int id)
        {
          if (_context.Collection == null)
          {
              return NotFound();
          }
            var collection = await _context.Collection.FindAsync(id);

            if (collection == null)
            {
                return NotFound();
            }

            return collection;
        }

        // PUT: api/Collection/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollection(int id, Collection collection)
        {
            if (id != collection.CollectionId)
            {
                return BadRequest();
            }

            _context.Entry(collection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionExists(id))
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

        // POST: api/Collection
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Collection>> PostCollection(CollectionCreateDto dto)
        {
            var collection = new Collection
            {
                UserId = dto.UserId,
                BookId = dto.BookId
            };

            _context.Collection.Add(collection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCollection", new { id = collection.CollectionId }, collection);
        }

        // DELETE: api/Collection/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id, [FromQuery] int userId)
        {
            var collection = await _context.Collection.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }

            if (collection.UserId != userId)
            {
                return Forbid();
            }

            _context.Collection.Remove(collection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CollectionExists(int id)
        {
            return (_context.Collection?.Any(e => e.CollectionId == id)).GetValueOrDefault();
        }
    }
}
