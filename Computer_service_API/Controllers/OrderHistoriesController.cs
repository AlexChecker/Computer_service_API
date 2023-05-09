using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Computer_service_API.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Computer_service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoriesController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public OrderHistoriesController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/OrderHistories
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<OrderHistory>>> GetOrderHistories(int page)
        {
          if (_context.OrderHistories == null)
          {
              return NotFound();
          }
            if (_context.OrderHistories.Count() < 10)
                return await _context.OrderHistories.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.OrderHistories.Count()) return NoContent();
                List<OrderHistory> acs = new List<OrderHistory>();

                for (int i = (int)(page * 10); i < _context.OrderHistories.Count(); i++)
                {
                    acs.Add(_context.OrderHistories.ToArray()[i]);
                }
                return acs;
            }
        }

        // GET: api/OrderHistories/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<OrderHistory>> GetOrderHistory(int? id)
        {
          if (_context.OrderHistories == null)
          {
              return NotFound();
          }
            var orderHistory = await _context.OrderHistories.FindAsync(id);

            if (orderHistory == null)
            {
                return NotFound();
            }

            return orderHistory;
        }

        // PUT: api/OrderHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutOrderHistory(int? id, OrderHistory orderHistory)
        {
            if (id != orderHistory.HistId)
            {
                return BadRequest();
            }

            _context.Entry(orderHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderHistoryExists(id))
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

        // POST: api/OrderHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<OrderHistory>> PostOrderHistory(OrderHistory orderHistory)
        {
          if (_context.OrderHistories == null)
          {
              return Problem("Entity set 'Computer_serviceContext.OrderHistories'  is null.");
          }
            _context.OrderHistories.Add(orderHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderHistory", new { id = orderHistory.HistId }, orderHistory);
        }

        // DELETE: api/OrderHistories/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteOrderHistory(int? id)
        {
            if (_context.OrderHistories == null)
            {
                return NotFound();
            }
            var orderHistory = await _context.OrderHistories.FindAsync(id);
            if (orderHistory == null)
            {
                return NotFound();
            }

            _context.OrderHistories.Remove(orderHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderHistoryExists(int? id)
        {
            return (_context.OrderHistories?.Any(e => e.HistId == id)).GetValueOrDefault();
        }
    }
}
