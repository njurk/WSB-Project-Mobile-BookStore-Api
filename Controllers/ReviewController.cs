using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Model.Entities;
using BookStoreApi.Model.Contexts;
using BookStoreApi.Models.DTOs;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public ReviewController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReview()
        {
          if (_context.Review == null)
          {
              return NotFound();
          }
            return await _context.Review.ToListAsync();
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
          if (_context.Review == null)
          {
              return NotFound();
          }
            var review = await _context.Review.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUser(int userId)
        {
            if (_context.Review == null)
            {
                return NotFound();
            }

            var reviews = await _context.Review
                .Where(r => r.UserId == userId)
                .Include(r => r.Book)
                .ToListAsync();

            return reviews;
        }

        // PUT: api/Review/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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
        public async Task<IActionResult> PatchReview(int id, [FromBody] ReviewPatchDto patchDto)
        {
            var review = await _context.Review
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
                return NotFound();

            if (patchDto.UserId != review.UserId)
                return Forbid();

            if (patchDto.Rating.HasValue)
                review.Rating = patchDto.Rating.Value;

            if (patchDto.Comment != null)
                review.Comment = patchDto.Comment;

            await _context.SaveChangesAsync();

            return Ok(review);
        }

        // POST: api/Review
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(ReviewCreateDto dto)
        {
            var review = new Review
            {
                BookId = dto.BookId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                DateCreated = DateTime.UtcNow
            };

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            await _context.Entry(review).Reference(r => r.User).LoadAsync();

            return CreatedAtAction("GetReview", new { id = review.ReviewId }, review);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id, [FromQuery] int userId)
        {
            var review = await _context.Review.FindAsync(id);
            if (review == null)
                return NotFound();

            if (review.UserId != userId)
                return Forbid();

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return (_context.Review?.Any(e => e.ReviewId == id)).GetValueOrDefault();
        }
    }
}
