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
    public class BookGenreController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public BookGenreController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/BookGenre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookGenreDto>>> GetBookGenres()
        {
            if (_context.BookGenre == null)
            {
                return NotFound();
            }

            var data = await _context.BookGenre
                .Select(bg => new BookGenreDto
                {
                    BookId = bg.BookId,
                    GenreId = bg.GenreId
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/BookGenre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookGenre>> GetBookGenre(int id)
        {
          if (_context.BookGenre == null)
          {
              return NotFound();
          }
            var bookGenre = await _context.BookGenre.FindAsync(id);

            if (bookGenre == null)
            {
                return NotFound();
            }

            return bookGenre;
        }

        // PUT: api/BookGenre/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookGenre(int id, BookGenre bookGenre)
        {
            if (id != bookGenre.BookGenreId)
            {
                return BadRequest();
            }

            _context.Entry(bookGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookGenreExists(id))
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

        // POST: api/BookGenre
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookGenre>> PostBookGenre(BookGenre bookGenre)
        {
          if (_context.BookGenre == null)
          {
              return Problem("Entity set 'BookStorePMABContext.BookGenre'  is null.");
          }
            _context.BookGenre.Add(bookGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookGenre", new { id = bookGenre.BookGenreId }, bookGenre);
        }

        // DELETE: api/BookGenre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookGenre(int id)
        {
            if (_context.BookGenre == null)
            {
                return NotFound();
            }
            var bookGenre = await _context.BookGenre.FindAsync(id);
            if (bookGenre == null)
            {
                return NotFound();
            }

            _context.BookGenre.Remove(bookGenre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookGenreExists(int id)
        {
            return (_context.BookGenre?.Any(e => e.BookGenreId == id)).GetValueOrDefault();
        }
    }
}
