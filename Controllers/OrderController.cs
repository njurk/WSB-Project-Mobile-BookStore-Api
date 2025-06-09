using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Model.Entities;
using BookStoreApi.Model.Contexts;
using BookStoreApi.Models.DTOs;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public OrderController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/Order/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUser(int userId)
        {
            if (_context.Order == null)
                return NotFound();

            var orders = await _context.Order
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryType)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    OrderStatusName = o.OrderStatus.Name,
                    DeliveryTypeName = o.DeliveryType.Name,
                    DeliveryFee = o.DeliveryType != null ? o.DeliveryType.Fee : 0m,
                    Street = o.Street,
                    City = o.City,
                    PostalCode = o.PostalCode,
                    TotalPrice = o.OrderItems.Sum(oi => oi.PriceAtPurchase * oi.Quantity) + (o.DeliveryType != null ? o.DeliveryType.Fee : 0m),
                    TotalItemCount = o.OrderItems.Sum(oi => oi.Quantity),
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.OrderItemId,
                        BookId = oi.BookId,
                        BookTitle = oi.Book.Title,
                        Quantity = oi.Quantity,
                        PriceAtPurchase = oi.PriceAtPurchase
                    }).ToList()
                })
                .ToListAsync();

            return orders;
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Order
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Include(o => o.DeliveryType)
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    OrderStatusName = o.OrderStatus.Name,
                    DeliveryTypeName = o.DeliveryType.Name,
                    DeliveryFee = o.DeliveryType != null ? o.DeliveryType.Fee : 0m,
                    Street = o.Street,
                    City = o.City,
                    PostalCode = o.PostalCode,
                    TotalPrice = o.OrderItems.Sum(oi => oi.PriceAtPurchase * oi.Quantity) + (o.DeliveryType != null ? o.DeliveryType.Fee : 0m),
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        OrderItemId = oi.OrderItemId,
                        BookId = oi.BookId,
                        BookTitle = oi.Book.Title,
                        Quantity = oi.Quantity,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        ImageUrl = oi.Book.ImageUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound();

            return order;
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
                return BadRequest();

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder(CreateOrderDto createOrderDto)
        {
            var bookIds = createOrderDto.OrderItems.Select(oi => oi.BookId).Distinct().ToList();
            var books = await _context.Book.Where(b => bookIds.Contains(b.BookId)).ToListAsync();

            if (books.Count != bookIds.Count)
            {
                var missingIds = bookIds.Except(books.Select(b => b.BookId));
                return BadRequest($"Books not found: {string.Join(", ", missingIds)}");
            }

            var deliveryType = await _context.DeliveryType.FindAsync(createOrderDto.DeliveryTypeId);
            if (deliveryType == null)
                return BadRequest($"DeliveryType {createOrderDto.DeliveryTypeId} not found");

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                OrderDate = DateTime.UtcNow,
                OrderStatusId = 1,
                Street = createOrderDto.Street,
                City = createOrderDto.City,
                PostalCode = createOrderDto.PostalCode,
                DeliveryTypeId = createOrderDto.DeliveryTypeId,
                OrderItems = new List<OrderItem>()
            };

            foreach (var itemDto in createOrderDto.OrderItems)
            {
                var book = books.FirstOrDefault(b => b.BookId == itemDto.BookId);
                if (book == null)
                    return BadRequest($"Book with ID {itemDto.BookId} not found");

                order.OrderItems.Add(new OrderItem
                {
                    BookId = book.BookId,
                    Quantity = itemDto.Quantity,
                    PriceAtPurchase = book.Price
                });
            }

            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            var orderWithDetails = await _context.Order
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryType)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            var orderDto = new OrderDto
            {
                OrderId = orderWithDetails.OrderId,
                UserId = orderWithDetails.UserId,
                OrderDate = orderWithDetails.OrderDate,
                OrderStatusName = orderWithDetails.OrderStatus.Name,
                DeliveryTypeName = orderWithDetails.DeliveryType.Name,
                DeliveryFee = orderWithDetails.DeliveryType.Fee,
                Street = orderWithDetails.Street,
                City = orderWithDetails.City,
                PostalCode = orderWithDetails.PostalCode,
                TotalPrice = orderWithDetails.OrderItems.Sum(oi => oi.PriceAtPurchase * oi.Quantity) + orderWithDetails.DeliveryType.Fee,
                OrderItems = orderWithDetails.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    BookId = oi.BookId,
                    BookTitle = books.First(b => b.BookId == oi.BookId).Title,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Order == null)
                return NotFound();

            var order = await _context.Order.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Order?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
