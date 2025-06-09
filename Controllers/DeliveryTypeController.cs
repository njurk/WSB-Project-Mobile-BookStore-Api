using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApi.Model.Contexts;
using BookStoreApi.Model.Entities;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly BookStorePMABContext _context;

        public DeliveryTypeController(BookStorePMABContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryType>>> GetDeliveryType()
        {
          if (_context.DeliveryType == null)
          {
              return NotFound();
          }
            return await _context.DeliveryType.ToListAsync();
        }

        // GET: api/DeliveryType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryType>> GetDeliveryType(int id)
        {
          if (_context.DeliveryType == null)
          {
              return NotFound();
          }
            var deliveryType = await _context.DeliveryType.FindAsync(id);

            if (deliveryType == null)
            {
                return NotFound();
            }

            return deliveryType;
        }

        // PUT: api/DeliveryType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryType(int id, DeliveryType deliveryType)
        {
            if (id != deliveryType.DeliveryTypeId)
            {
                return BadRequest();
            }

            _context.Entry(deliveryType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryTypeExists(id))
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

        // POST: api/DeliveryType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryType>> PostDeliveryType(DeliveryType deliveryType)
        {
          if (_context.DeliveryType == null)
          {
              return Problem("Entity set 'BookStorePMABContext.DeliveryType'  is null.");
          }
            _context.DeliveryType.Add(deliveryType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryType", new { id = deliveryType.DeliveryTypeId }, deliveryType);
        }

        // DELETE: api/DeliveryType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryType(int id)
        {
            if (_context.DeliveryType == null)
            {
                return NotFound();
            }
            var deliveryType = await _context.DeliveryType.FindAsync(id);
            if (deliveryType == null)
            {
                return NotFound();
            }

            _context.DeliveryType.Remove(deliveryType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryTypeExists(int id)
        {
            return (_context.DeliveryType?.Any(e => e.DeliveryTypeId == id)).GetValueOrDefault();
        }
    }
}
