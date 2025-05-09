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
    public class CartController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public CartController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartItems([FromQuery] int userId)
        {
            if (_context.Cart == null)
                return NotFound();

            var cartItems = await _context.Cart
                .Where(c => c.UserId == userId)
                .Include(c => c.Book)
                .ToListAsync();

            return cartItems;
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCartItem(int id)
        {
            var cartItem = await _context.Cart
                .Include(c => c.Book)
                .FirstOrDefaultAsync(c => c.CartId == id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        // PUT: api/Cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartQuantity(int id, CartUpdateDto dto)
        {
            var cartItem = await _context.Cart.FindAsync(id);
            if (cartItem == null)
                return NotFound();

            cartItem.Quantity = dto.Quantity;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCartItem(CartCreateDto dto)
        {
            // Check if the item already exists in the cart for the user
            var existingCartItem = await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.BookId == dto.BookId);

            if (existingCartItem != null)
            {
                // If the item exists, increase the quantity
                existingCartItem.Quantity += dto.Quantity;
                _context.Entry(existingCartItem).State = EntityState.Modified;
            }
            else
            {
                // If the item doesn't exist, create a new cart item
                var cartItem = new Cart
                {
                    UserId = dto.UserId,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity
                };

                _context.Cart.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Load the cart item including the Book navigation property
            var fullCartItem = await _context.Cart
                .Include(c => c.Book)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId && c.BookId == dto.BookId);

            if (fullCartItem == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetCartItem), new { id = fullCartItem.CartId }, fullCartItem);
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (_context.Cart == null)
            {
                return NotFound();
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return (_context.Cart?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
