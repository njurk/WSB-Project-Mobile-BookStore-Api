using BookStoreApi.Model.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models.Services
{
    public class BookService
    {
        private readonly BookStorePMABContext _context;

        public BookService(BookStorePMABContext context)
        {
            _context = context;
        }

        public async Task<object?> GetBookWithAverageRatingAsync(int bookId)
        {
            if (_context.Book == null)
                return null;

            var bookWithAvg = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(b => b.Reviews).ThenInclude(r => r.User)
                .Where(b => b.BookId == bookId)
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Price,
                    b.ImageUrl,
                    b.Description,
                    b.YearPublished,
                    b.NumberOfPages,
                    b.Language,
                    b.Isbn,
                    Author = b.Author,
                    BookGenre = b.BookGenres,
                    Review = b.Reviews,
                    AverageRating = b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0
                })
                .FirstOrDefaultAsync();

            return bookWithAvg;
        }
    }

}
