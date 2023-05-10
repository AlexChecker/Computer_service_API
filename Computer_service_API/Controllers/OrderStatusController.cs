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
    public class OrderStatusController : ControllerBase
    {
        private readonly Computer_serviceContext _context;

        public OrderStatusController(Computer_serviceContext context)
        {
            _context = context;
        }

        // GET: api/OrderStatus
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetOrderStatuses(int page)
        {
          if (_context.OrderStatuses == null)
          {
              return NotFound();
          }
            if (_context.OrderStatuses.Count() < 10)
                return await _context.OrderStatuses.ToListAsync();
            else
            {
                if (page == null) page = 1;
                if (page * 10 > _context.OrderStatuses.Count()) return NoContent();
                List<OrderStatus> acs = new List<OrderStatus>();

                for (int i = (int)(page * 10); i < page * 10 + 10; i++)
                {
                    try
                    {
                        acs.Add(_context.OrderStatuses.ToArray()[i]);
                    }
                    catch { break; }
                }
                return acs;
            }
        }

        // GET: api/OrderStatus/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<OrderStatus>> GetOrderStatus(int? id)
        {
          if (_context.OrderStatuses == null)
          {
              return NotFound();
          }
            var orderStatus = await _context.OrderStatuses.FindAsync(id);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return orderStatus;
        }

        // PUT: api/OrderStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Employee")]
        public async Task<IActionResult> PutOrderStatus(int? id, OrderStatus orderStatus)
        {
            if (id != orderStatus.StatusId)
            {
                return BadRequest();
            }

            _context.Entry(orderStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusExists(id))
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

        // POST: api/OrderStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Employee")]
        public async Task<ActionResult<OrderStatus>> PostOrderStatus(OrderStatus orderStatus)
        {
          if (_context.OrderStatuses == null)
          {
              return Problem("Entity set 'Computer_serviceContext.OrderStatuses'  is null.");
          }
            _context.OrderStatuses.Add(orderStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderStatusExists(orderStatus.StatusId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderStatus", new { id = orderStatus.StatusId }, orderStatus);
        }

        // DELETE: api/OrderStatus/5
        [HttpDelete("{id}"), Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteOrderStatus(int? id)
        {
            if (_context.OrderStatuses == null)
            {
                return NotFound();
            }
            var orderStatus = await _context.OrderStatuses.FindAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            _context.OrderStatuses.Remove(orderStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderStatusExists(int? id)
        {
            return (_context.OrderStatuses?.Any(e => e.StatusId == id)).GetValueOrDefault();
        }
    }
}
